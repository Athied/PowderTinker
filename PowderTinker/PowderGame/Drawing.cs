using PowderGame.Materials;
using Raylib_cs;
using System.IO;
using System.Text;

using static PowderGame.Program;
using static PowderGame.Cells;

namespace PowderGame
{
    public static class Drawing
    {
        private static readonly int FontSize = 18;

        private static readonly Color InfoColor = Color.WHITE;
        private static readonly Color BorderColor = Color.RED;
        private static readonly Color GridMainColor = Color.BLUE;
        private static readonly Color GridOffColor = Color.DARKBLUE;
        private static readonly Color SleepingChunksColor = new Color(0, 128, 128, 32);
        private static readonly Color AwakeChunksColor = new Color(0, 128, 128, 128);
        private static readonly Color CellVelocityColor = Color.PINK;

        public static void DrawFrame()
        {
            DrawCells();
            DrawDebugContent();
            DrawHUD();
        }

        public static void DrawCells()
        {
            foreach (Cell cell in CellsEnumerable)
            {
                cell.Draw();
            }
        }

        public static void DrawDebugContent()
        {
            //DrawGrid();
            //DrawBorder();

            DrawChunksDebug();
            DrawCellDebug();
        }

        public static void DrawHUD()
        {
            Cell? hoveredCell = MouseInput.GetHoveredCell();

            string cell = hoveredCell != null ? $"Cell: {hoveredCell.OccupyingMaterial.Name} ({hoveredCell.Index.X}x{hoveredCell.Index.Y})" : "Cell: unknown";
            string chunk = hoveredCell != null ? $"{hoveredCell.Chunk.ChunkIndex.X}x{hoveredCell.Chunk.ChunkIndex.Y}" : "no chunk";
            string selectedMat = $"Using: {MouseInput.SelectedMaterialName}";
            string brushSize = $"Brush size: {MouseInput.BrushSize}";
            string brushDensity = $"Brush density: {MouseInput.BrushDensity:n1}";
            string physicsTime = $"Physics Time (ms): {Physics.AveragePhysicsTimeTaken:n2}";
            string activeCells = $"Active Cells: {Physics.ActiveCells}";
            string sand = $"Cells (Sand): {CellsEnumerable.Where(c => c.OccupyingMaterial.Name == "Sand").Count()}";
            string water = $"Cells (Water): {CellsEnumerable.Where(c => c.OccupyingMaterial.Name == "Water").Count()}";

            StringBuilder sb = new StringBuilder();
            sb.Append(cell + $" ({chunk})" + "\n");
            sb.Append(selectedMat + "\n");
            sb.Append(brushSize + "\n");
            sb.Append(brushDensity + "\n");
            sb.Append(physicsTime + "\n");
            sb.Append(activeCells + "\n\n");
            sb.Append(sand + "\n");
            sb.Append(water + "\n");

            Raylib.DrawFPS(20, 20);
            Raylib.DrawText(sb.ToString(), 20, 60, FontSize, InfoColor);
        }

        static void DrawBorder()
        {
            Raylib.DrawRectangleLines(GameCorners.Left, GameCorners.Top, GameW, GameH, BorderColor);
        }

        static void DrawGrid()
        {
            for (int columnPos = GameCorners.Left + CellSize; columnPos < GameCorners.Right; columnPos += CellSize)
            {
                Color col = columnPos % 4 == 0 ? GridMainColor : GridOffColor;
                Raylib.DrawLine(columnPos, GameCorners.Top, columnPos, GameCorners.Bottom, col);
            }

            for (int rowPos = GameCorners.Top + CellSize; rowPos < GameCorners.Bottom; rowPos += CellSize)
            {
                Color col = rowPos % 4 == 0 ? GridMainColor : GridOffColor;
                Raylib.DrawLine(GameCorners.Left, rowPos, GameCorners.Right, rowPos, col);
            }
        }

        static void DrawChunksDebug()
        {
            foreach (Chunking.Chunk chunk in Chunking.Chunks)
            {
                Color col = chunk.Sleeping ? SleepingChunksColor : AwakeChunksColor;

                Raylib.DrawRectangleLines(chunk.ScreenPos.X, chunk.ScreenPos.Y, CellSize * Chunking.ChunkSize, CellSize * Chunking.ChunkSize, col);
            }
        }

        static void DrawCellDebug()
        {
            IEnumerable<Cell> cells = CellsEnumerable.Where(c => c.OccupyingMaterial.MaterialType != MaterialTypes.None && c.OccupyingMaterial.DrawDebugInfo);

            foreach (Cell cell in cells)
            {
                IMaterial m = cell.OccupyingMaterial;
                Raylib.DrawText($"{(int)m.Velocity.X}x{(int)m.Velocity.Y}", cell.ScreenPos.X, cell.ScreenPos.Y - CellSize, 16, CellVelocityColor);
            }
        }
    }
}
