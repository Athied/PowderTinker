using PowderGame.Materials;
using Raylib_cs;
using System.Numerics;

using static PowderGame.Cells;
using static PowderGame.Chunking;

namespace PowderGame
{
    public static class Physics
    {
        public static class ExternalForces
        {
            public static Force Gravity = new Force(0, 9.8f);
            public static readonly float AirDensity = 1;
        }

        private static readonly List<float> AvgMilliseconds = new List<float>();

        public static float AveragePhysicsTimeTaken { get; private set; }
        public static int ActiveCells { get; private set; }

        public static readonly float PhysicsSpeedMult = 1f;

        private static bool UseChunking = true;

        public static void RunPhysicsOnAllCells()
        {
            DateTime t = DateTime.Now;

            //IEnumerable<Cell> cellsToUpdate = CellsEnumerable.Where(c => (!c.Chunk.Sleeping || !UseChunking) && c.OccupyingMaterial.MaterialType != MaterialTypes.None);
            //var cellsToUpdate = CellsEnumerable.ToList().FindAll(c => (!c.Chunk.Sleeping || !UseChunking) && c.OccupyingMaterial.MaterialType != MaterialTypes.None);
            
            // method a

            var cellsToUpdate = Chunks.ToList().FindAll(x => !x.Sleeping || !UseChunking).SelectMany(y => y.GetContainedCells());

            ActiveCells = cellsToUpdate.Count();

            foreach (Chunk chunk in Chunks)
            {
                chunk.UpdateSleepState();
            }

            Parallel.ForEach(cellsToUpdate, cell =>
            {
                if (cell.OccupyingMaterial.MaterialType == MaterialTypes.None) return;
                cell.OccupyingMaterial.RunPhysics(cell);
            });

            // method b

            //int ac = 0;
            ////Parallel.ForEach(Chunks, chunk =>
            //foreach (var chunk in Chunks.Reverse())
            //{
            //    chunk.UpdateSleepState();

            //    if (!chunk.Sleeping || !UseChunking)
            //    {
            //        //var cells = chunk.GetContainedCells().Where(c => c.OccupyingMaterial.MaterialType != MaterialTypes.None);

            //        foreach (var cell in chunk.GetContainedCells().Reverse())
            //        {
            //            if (cell.OccupyingMaterial.MaterialType == MaterialTypes.None) continue;
            //            cell.OccupyingMaterial.RunPhysics(cell);

            //            ac++;
            //        };
            //    }
            //};

            //ActiveCells = ac;

            if (AvgMilliseconds.Count > 180) AvgMilliseconds.RemoveAt(0);

            AvgMilliseconds.Add((float)(DateTime.Now - t).TotalMilliseconds);

            AveragePhysicsTimeTaken = (float)AvgMilliseconds.Average();
        }

        public static void ApplyExternalForces(Cell cell)
        {
            IMaterial m = cell.OccupyingMaterial;

            // Gravity & Drag

            bool applyGravityX = false;
            bool applyGravityY = false;

            if (ExternalForces.Gravity.X < 0) applyGravityX = QueryMaterial(cell, -1, 0, MaterialTypes.None);
            if (ExternalForces.Gravity.X > 0) applyGravityX = QueryMaterial(cell, 1, 0, MaterialTypes.None);
            if (ExternalForces.Gravity.Y < 0) applyGravityY = QueryMaterial(cell, 0, -1, MaterialTypes.None);
            if (ExternalForces.Gravity.Y > 0) applyGravityY = QueryMaterial(cell, 0, 1, MaterialTypes.None);

            if (applyGravityX || applyGravityY)
            {
                float xGrav = applyGravityX ? ExternalForces.Gravity.X / Program.FPS : 0;
                float yGrav = applyGravityY ? ExternalForces.Gravity.Y / Program.FPS : 0;
                Vector2 gravityPerFrame = new Vector2(xGrav, yGrav);

                // Faking terminal velocity and drag
                Vector2 gravity = (gravityPerFrame * m.Density) / ExternalForces.AirDensity;

                float terminalVelocityX = gravity.X * m.DragResistance * ExternalForces.AirDensity;
                float terminalVelocityY = gravity.Y * m.DragResistance * ExternalForces.AirDensity;
                
                m.TerminalVelocity = new Force(terminalVelocityX, terminalVelocityY);

                m.Velocity.AddRaw(gravity);

                if (m.Velocity.X > m.TerminalVelocity.X) m.Velocity.X = terminalVelocityX;
                if (m.Velocity.Y > m.TerminalVelocity.Y) m.Velocity.Y = terminalVelocityY;
            }

            // Friction

            MaterialTypes[] frictionSurfaces = new MaterialTypes[] { MaterialTypes.Solid, MaterialTypes.Powder, MaterialTypes.OutsideMap };

            if (QueryMaterial(cell, 0, 1, frictionSurfaces) || QueryMaterial(cell, 0, -1, frictionSurfaces))
            {
                m.Velocity.Reduce(1, 0);
            }

            if (QueryMaterial(cell, 1, 0, frictionSurfaces) || QueryMaterial(cell, -1, 0, frictionSurfaces))
            {
                m.Velocity.Reduce(0, 1);
            }

            // Rapid velocity loss if suffocated
            // Important for chunking
            if (Cells.IsSurroundedBySolids(cell))
            {
                m.Velocity.Reduce(50, 50);
            }
        }
    }
}
