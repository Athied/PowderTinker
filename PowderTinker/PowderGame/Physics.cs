using PowderGame.Materials;
using Raylib_cs;
using System.Numerics;
using static PowderGame.Program;

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

        public static void RunPhysicsOnAllCells()
        {
            DateTime t = DateTime.Now;

            IEnumerable<Cell> cells = G_Cells.Where(c => c.OccupyingMaterial.MaterialType != MaterialTypes.None).Reverse();

            ActiveCells = cells.Count();

            Parallel.ForEach(cells, cell =>
            {
                cell.OccupyingMaterial.RunPhysics(cell);
            });

            if (AvgMilliseconds.Count > 180) AvgMilliseconds.RemoveAt(0);

            AvgMilliseconds.Add((float)(DateTime.Now - t).TotalMilliseconds);

            AveragePhysicsTimeTaken = (float)AvgMilliseconds.Average();
        }

        public static void ApplyExternalForces(Cell cell)
        {
            IMaterial m = cell.OccupyingMaterial;

            int FPS = Raylib.GetFPS();

            // Gravity & Drag

            bool applyGravityX = false;
            bool applyGravityY = false;

            if (ExternalForces.Gravity.X < 0) applyGravityX = Helpers.CheckForMaterialsRelative(cell, -1, 0, MaterialTypes.None);
            if (ExternalForces.Gravity.X > 0) applyGravityX = Helpers.CheckForMaterialsRelative(cell, 1, 0, MaterialTypes.None);
            if (ExternalForces.Gravity.Y < 0) applyGravityY = Helpers.CheckForMaterialsRelative(cell, 0, -1, MaterialTypes.None);
            if (ExternalForces.Gravity.Y > 0) applyGravityY = Helpers.CheckForMaterialsRelative(cell, 0, 1, MaterialTypes.None);

            if (applyGravityX || applyGravityY)
            {
                float xGrav = applyGravityX ? ExternalForces.Gravity.X / FPS : 0;
                float yGrav = applyGravityY ? ExternalForces.Gravity.Y / FPS : 0;
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

            if (Helpers.CheckForMaterialsRelative(cell, 0, 1, frictionSurfaces) || Helpers.CheckForMaterialsRelative(cell, 0, -1, frictionSurfaces))
            {
                m.Velocity.Reduce(1, 0);
            }

            if (Helpers.CheckForMaterialsRelative(cell, 1, 0, frictionSurfaces) || Helpers.CheckForMaterialsRelative(cell, -1, 0, frictionSurfaces))
            {
                m.Velocity.Reduce(0, 1);
            }
        }
    }
}
