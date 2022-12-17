using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PowderGame
{
    static class CellMovement
    {
        public static bool TryTeleport(Cell cell, MaterialTypes[] validTypes, Position index, bool swapCells)
        {
            Cell? target = Helpers.GetCellAtIndex(index.X, index.Y);
            if (target == null) return false;

            if (validTypes.Contains(target.OccupyingMaterial.MaterialType))
            {
                if (!swapCells) target.ReplaceMaterial(cell, new Materials.Void());
                else target.ReplaceMaterial(cell, target.OccupyingMaterial);

                return true;
            }

            return false;
        }

        public static bool TryTeleportRelative(Cell cell, MaterialTypes[] validTypes, Position offset, bool swapCells)
        {
            Position moveToPos = new Position(cell.Index.X + offset.X, cell.Index.Y + offset.Y);
            return TryTeleport(cell, validTypes, moveToPos, swapCells);
        }

        public static int TryMoveAlongPath(Cell cell, MaterialTypes[] validTypes, bool swapCells, Position[] pathPoints)
        {
            int steps = 0;

            foreach (Position offset in pathPoints)
            {
                Position moveToPos = new Position(cell.Index.X + offset.X, cell.Index.Y + offset.Y);
                if (TryTeleport(cell, validTypes, moveToPos, swapCells)) return steps;

                steps++;
            }

            return -1;
        }

        public static void FindOptimalPath(Position p1, Position p2)
        {
            if (p1.Equals(p2)) return;

            int xDiff = p1.X - p2.X;
            int yDiff = p1.X - p2.X;


        }
    }
}
