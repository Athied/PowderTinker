using Raylib_cs;
using System;
using System.Numerics;
using static PowderGame.Program;

namespace PowderGame
{
    public static class Physics
    {
        public static class ExternalForces
        {
            public static float Gravity = 9.8f;
        }

        private static List<float> AvgMilliseconds = new List<float>();

        public static float AveragePhysicsTimeTaken { get; private set; }
        public static int ActiveCells { get; private set; }

        public static void RunPhysicsOnAllCells()
        {
            DateTime t = DateTime.Now;

            IEnumerable<Cell> cells = G_Cells.Where(c => c.Material.MaterialType != MaterialTypes.None);

            ActiveCells = cells.Count();

            Parallel.ForEach(G_Cells, cell =>
            {
                cell.Material.PhysicsTimer += Raylib.GetFrameTime() * G_PhysicsRate * cell.Material.OverallSpeed;

                if (cell.Material.PhysicsTimer > G_PhysicsTimerTarget)
                {
                    cell.Material.PhysicsTimer = 0;

                    switch (cell.Material.MaterialType)
                    {
                        case MaterialTypes.Powder: RunPowderPhysics(cell); break;
                        case MaterialTypes.Liquid: RunLiquidPhysics(cell); break;
                        case MaterialTypes.Solid: RunSolidPhysics(cell); break;
                    }
                }
            });

            AvgMilliseconds.Add((float)(DateTime.Now - t).TotalMilliseconds);

            if (AvgMilliseconds.Count > 180)
            {
                AvgMilliseconds.RemoveAt(0);
            }

            AveragePhysicsTimeTaken = (float)AvgMilliseconds.Average();
        }

        private static void RunPowderPhysics(Cell cell)
        {
            // Rules:
            // A: Powders will displace liquids rather than sit on top of them
            // 0: If there is a valid space n cells down, move down
            // 1: If there is a valid space n cells down and to the left, move there
            // 2: If there is a valid space n cells down and to the right, move there

            MaterialTypes[] validTypes = new MaterialTypes[] { MaterialTypes.None, MaterialTypes.Liquid };

            CellMovement.TryMoveAlongPath(cell, validTypes, true, new (int, int)[]
            {
                (0, 1),
                (-1, 1),
                (1, 1)
            });
        }

        private static void RunLiquidPhysics(Cell cell)
        {
            // Rules:
            // 0: If there is a valid space n cells down, move down
            // 1: If there is a valid space n cells down and to the left, move there
            // 2: If there is a valid space n cells down and to the right, move there
            // 3: If there is a valid space n cells to the left and right, stop.

            // 4: Otherwise, if there is a valid space n cells to the left, move there
            // 5: If there is a valid space n cells to the right, move there

            MaterialTypes[] validTypes = new MaterialTypes[] { MaterialTypes.None };

            int a = CellMovement.TryMoveAlongPath(cell, validTypes, false, new (int, int)[]
            {
                (0, 1),
                (-1, 1),
                (1, 1),
            });

            // If any of previous 3 movement checks succeeded, do not move.
            if (a != -1) return;

            // If both the left and right spaces are empty, do not move.
            Cell? l = Helpers.GetCellAtIndex(cell.IndexX - 1, cell.IndexY);
            if (l == null) return;

            Cell? r = Helpers.GetCellAtIndex(cell.IndexX + 1, cell.IndexY);
            if (r == null) return;

            if (l.Material.MaterialType == MaterialTypes.None && r.Material.MaterialType == MaterialTypes.None) return;

            // Otherwise, try to move left, then right.
            validTypes = new MaterialTypes[] { MaterialTypes.None };

            CellMovement.TryMoveAlongPath(cell, validTypes, false, new (int, int)[]
            {
                (-1, 0),
                (1, 0)
            });
        }

        private static void RunSolidPhysics(Cell cell)
        {
            // Rules:
            // A: Solids will displace liquids rather than sit on top of them
            // B: Solids will displace powders rather than sit on top of them
            // 0: If there is a valid space n cells down, move down

            MaterialTypes[] validTypes = new MaterialTypes[] { MaterialTypes.None, MaterialTypes.Liquid, MaterialTypes.Powder };

            CellMovement.TryMoveAlongPath(cell, validTypes, true, new (int, int)[]
            {
                (0, 1)
            });
        }
    }
}
