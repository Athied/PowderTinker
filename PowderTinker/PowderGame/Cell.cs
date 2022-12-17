using Raylib_cs;
using static PowderGame.Program;

namespace PowderGame
{
    public class Cell
    {
        public Cell(int x, int y, Materials.IMaterial _material)
        {
            Index.X = x;
            Index.Y = y;

            SetMaterial(_material);
        }

        public Cell(Position index, Materials.IMaterial _material)
        {
            Index = index;

            SetMaterial(_material);
        }

        public readonly Position Index;
        public Position ScreenPos { get { return new Position(GameCorners.Left + Index.X * G_CellSize, GameCorners.Top + Index.Y * G_CellSize); } }
        public Position GridPos { get { return new Position(ScreenPos.X / G_CellSize, ScreenPos.Y / G_CellSize); } }

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
            Raylib.DrawRectangle(GridPos.X * G_CellSize, GridPos.Y * G_CellSize, G_CellSize, G_CellSize, color);
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
