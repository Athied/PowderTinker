﻿namespace PowderGame.Materials
{
    public abstract class Liquid : BaseMaterial
    {
        public sealed override MaterialTypes MaterialType { get { return MaterialTypes.Liquid; } }

        public override void RunPhysics(Cell cell)
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

            if (l.OccupyingMaterial.MaterialType == MaterialTypes.None && r.OccupyingMaterial.MaterialType == MaterialTypes.None) return;

            // Otherwise, try to move left, then right.
            validTypes = new MaterialTypes[] { MaterialTypes.None };

            CellMovement.TryMoveAlongPath(cell, validTypes, false, new (int, int)[]
            {
                (-1, 0),
                (1, 0)
            });
        }
    }
}