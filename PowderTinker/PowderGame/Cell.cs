using Raylib_cs;
using static PowderGame.Program;

namespace PowderGame
{
    public class Cell
    {
        public Cell(int _x, int _y, IMaterial _material)
        {
            IndexX = _x;
            IndexY = _y;

            material = _material;

            SetColor();
        }

        public readonly int IndexX;
        public readonly int IndexY;

        public int GridX { get { return (GameCorners.Left + IndexX * G_CellSize) / G_CellSize; } }
        public int GridY { get { return (GameCorners.Top + IndexY * G_CellSize) / G_CellSize; } }

        public int ScreenX { get { return GameCorners.Left + IndexX * G_CellSize; } }
        public int ScreenY { get { return GameCorners.Top + IndexY * G_CellSize; } }

        public int ActiveCellsIndex { get; set; }

        private Color color;
        public Color Color { get { return color; } }

        private IMaterial material;
        public IMaterial Material { get { return material; } set { material = value; SetColor(); } }

        public void SetColor()
        {
            int r = (int)Helpers.RandomRange(material.Colors.Min.r, material.Colors.Max.r);
            int g = (int)Helpers.RandomRange(material.Colors.Min.g, material.Colors.Max.g);
            int b = (int)Helpers.RandomRange(material.Colors.Min.b, material.Colors.Max.b);
            int a = (int)Helpers.RandomRange(material.Colors.Min.a, material.Colors.Max.a);

            color = new Color(r, g, b, a);
        }

        public void Draw()
        {
            if (Material.MaterialType == MaterialTypes.None) return;
            Raylib.DrawRectangle(GridX * G_CellSize, GridY * G_CellSize, G_CellSize, G_CellSize, color);
        }

        public void SwapMaterial(Cell replacementSource, IMaterial? newMaterial = null)
        {
            newMaterial ??= new Materials.Void();

            Material = replacementSource.Material;
            replacementSource.Material = newMaterial;
        }
    }
}
