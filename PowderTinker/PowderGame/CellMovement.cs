using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowderGame
{
    static class CellMovement
    {
        public static bool TryTeleport(Cell cell, MaterialTypes[] validTypes, int indexX, int indexY, bool swapCells)
        {
            Cell? target = Helpers.GetCellAtIndex(indexX, indexY);
            if (target == null) return false;

            if (validTypes.Contains(target.Material.MaterialType))
            {
                if (!swapCells) target.SwapMaterial(cell);
                else target.SwapMaterial(cell, target.Material);

                return true;
            }

            return false;
        }

        public static bool TryTeleportRelative(Cell cell, MaterialTypes[] validTypes, int offsetX, int offsetY, bool swapCells)
        {
            return TryTeleport(cell, validTypes, cell.IndexX + offsetX, cell.IndexY + offsetY, swapCells);
        }

        public static int TryMoveAlongPath(Cell cell, MaterialTypes[] validTypes, bool swapCells, (int offsetX, int offsetY)[] pathPoints)
        {
            int steps = 0;

            foreach ((int offsetX, int offsetY) in pathPoints)
            {
                if (TryTeleport(cell, validTypes, cell.IndexX + offsetX, cell.IndexY + offsetY, swapCells)) return steps;

                steps++;
            }

            return -1;
        }
    }
}
