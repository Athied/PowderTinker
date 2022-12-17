using Raylib_cs;
using static PowderGame.Program;

namespace PowderGame
{
    public class Cell
    {
        public Cell(int _x, int _y, Materials.IMaterial _material)
        {
            IndexX = _x;
            IndexY = _y;

            SetMaterial(_material);
        }

        public readonly int IndexX;
        public readonly int IndexY;

        public int GridX { get { return (GameCorners.Left + IndexX * G_CellSize) / G_CellSize; } }
        public int GridY { get { return (GameCorners.Top + IndexY * G_CellSize) / G_CellSize; } }

        public int ScreenX { get { return GameCorners.Left + IndexX * G_CellSize; } }
        public int ScreenY { get { return GameCorners.Top + IndexY * G_CellSize; } }

        public int ActiveCellsIndex { get; set; }

        private Color color = Color.RED;
        public Color Color { get { return color; } }

        public Materials.IMaterial OccupyingMaterial { get; private set; } = default!;

        private void SetColor()
        {
            int r = (int)Helpers.RandomRange(OccupyingMaterial.Colors.Min.r, OccupyingMaterial.Colors.Max.r);
            int g = (int)Helpers.RandomRange(OccupyingMaterial.Colors.Min.g, OccupyingMaterial.Colors.Max.g);
            int b = (int)Helpers.RandomRange(OccupyingMaterial.Colors.Min.b, OccupyingMaterial.Colors.Max.b);
            int a = (int)Helpers.RandomRange(OccupyingMaterial.Colors.Min.a, OccupyingMaterial.Colors.Max.a);

            color = new Color(r, g, b, a);
        }

        public void Draw()
        {
            if (OccupyingMaterial.MaterialType == MaterialTypes.None) return;
            Raylib.DrawRectangle(GridX * G_CellSize, GridY * G_CellSize, G_CellSize, G_CellSize, color);
        }

        public void SetMaterial(Materials.IMaterial material)
        {
            material ??= new Materials.Void();

            OccupyingMaterial = material;
            SetColor();
        }

        /// <summary> Takes the material from a cell and sets that cell's material to a given one. </summary>
        public void ReplaceMaterial(Cell replacementSource, Materials.IMaterial newMaterial)
        {
            SetMaterial(replacementSource.OccupyingMaterial);
            replacementSource.SetMaterial(newMaterial);
        }
    }
}
