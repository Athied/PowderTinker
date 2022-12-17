using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowderGame
{
    public static class Helpers
    {
        private static readonly Random rand = new Random();

        public static float RandomRange(float min, float max)
        {
            return (float)rand.NextDouble() * (max - min) + min;
        }

        public static Cell? GetCellAtIndex(int x, int y)
        {
            if (x >= Program.G_CellLookup.GetLength(0) || y >= Program.G_CellLookup.GetLength(1)) return null;
            if (x < 0 || y < 0) return null;

            return Program.G_CellLookup[x, y];
        }

        public static bool CheckForMaterialsRelative(Cell originCell, int offsetX, int offsetY, MaterialTypes materialType)
        {
            return CheckForMaterialsRelative(originCell, offsetX, offsetY, new MaterialTypes[] { materialType });
        }

        public static bool CheckForMaterialsRelative(Cell originCell, Position offset, MaterialTypes materialType)
        {
            return CheckForMaterialsRelative(originCell, offset.X, offset.Y, new MaterialTypes[] { materialType });
        }

        public static bool CheckForMaterialsRelative(Cell originCell, Position offset, MaterialTypes[] materialTypes)
        {
            return CheckForMaterialsRelative(originCell, offset.X, offset.Y, materialTypes);
        }

        public static bool CheckForMaterialsRelative(Cell originCell, int offsetX, int offsetY, MaterialTypes[] materialTypes)
        {
            Cell? foundCell = GetCellAtIndex(originCell.Index.X + offsetX, originCell.Index.Y + offsetY);
            if (foundCell == null) return materialTypes.Contains(MaterialTypes.OutsideMap);

            return materialTypes.Contains(foundCell.OccupyingMaterial.MaterialType);
        }

        public static bool CheckIfSurroundedByMaterials(Cell cell, MaterialTypes materialType)
        {
            return CheckIfSurroundedByMaterials(cell, new MaterialTypes[] { materialType });
        }

        public static bool CheckIfSurroundedByMaterials(Cell cell, MaterialTypes[] materialTypes)
        {
            if (!CheckForMaterialsRelative(cell, 0, -1, materialTypes)) return false;
            if (!CheckForMaterialsRelative(cell, 1, 0, materialTypes)) return false;
            if (!CheckForMaterialsRelative(cell, 0, 1, materialTypes)) return false;
            if (!CheckForMaterialsRelative(cell, 1, 0, materialTypes)) return false;

            return true;
        }

        public static bool CheckIfTouchingMaterials(Cell cell, MaterialTypes materialType)
        {
            return CheckIfTouchingMaterials(cell, new MaterialTypes[] { materialType });
        }

        public static bool CheckIfTouchingMaterials(Cell cell, MaterialTypes[] materialTypes)
        {
            if (!CheckForMaterialsRelative(cell, 0, -1, materialTypes)) return true;
            if (!CheckForMaterialsRelative(cell, 1, 0, materialTypes)) return true;
            if (!CheckForMaterialsRelative(cell, 0, 1, materialTypes)) return true;
            if (!CheckForMaterialsRelative(cell, 1, 0, materialTypes)) return true;

            return false;
        }
    }
}