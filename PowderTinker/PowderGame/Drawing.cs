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

        private static readonly Color ChunkColor = new Color(0, 128, 128, 128);

        public static void DrawDebugContent()
        {
            //DrawGrid();
            DrawBorder();

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
            Raylib.DrawText(sb.ToString(), 20, 60, FontSize, Color.WHITE);
        }

        public static void DrawCells()
        {
            foreach (Cell cell in CellsEnumerable)
            {
                cell.Draw();
            }
        }

        static void DrawBorder()
        {
            Raylib.DrawRectangleLines(GameCorners.Left, GameCorners.Top, GameW, GameH, Color.RED);
        }

        static void DrawGrid()
        {
            // Columns
            for (int columnPos = GameCorners.Left + CellSize; columnPos < GameCorners.Right; columnPos += CellSize)
            {
                Color col = columnPos % 4 == 0 ? Color.BLUE : Color.DARKBLUE;

                Raylib.DrawLine(columnPos, GameCorners.Top, columnPos, GameCorners.Bottom, col);
            }

            // Rows
            for (int rowPos = GameCorners.Top + CellSize; rowPos < GameCorners.Bottom; rowPos += CellSize)
            {
                Color col = rowPos % 4 == 0 ? Color.BLUE : Color.DARKBLUE;

                Raylib.DrawLine(GameCorners.Left, rowPos, GameCorners.Right, rowPos, col);
            }
        }

        static void DrawChunksDebug()
        {
            IEnumerable<Chunking.Chunk> awakeChunks = Chunking.Chunks.Where(x => !x.Sleeping);

            foreach (Chunking.Chunk chunk in awakeChunks)
            {
                Raylib.DrawRectangleLines(chunk.ScreenPos.X, chunk.ScreenPos.Y, CellSize * Chunking.ChunkSize, CellSize * Chunking.ChunkSize, ChunkColor);
            }
        }

        static void DrawCellDebug()
        {
            IEnumerable<Cell> cells = CellsEnumerable.Where(c => c.OccupyingMaterial.MaterialType != MaterialTypes.None && c.OccupyingMaterial.DrawDebugInfo);

            foreach (Cell cell in cells)
            {
                IMaterial m = cell.OccupyingMaterial;

                if (m.LastProjectedPath != null)
                {
                    for (int i = 0; i < m.LastProjectedPath.Length; i++)
                    {
                        Position pos = m.LastProjectedPath[i];

                        Cell? c = Cells.FindByIndex(pos.X, pos.Y);
                        if (c == null) continue;

                        Color col = i == 0 ? col = Color.BLUE : Color.RED;
                        if (i == m.LastProjectedPath.Length - 1) col = Color.GREEN;

                        Raylib.DrawRectangle(c.GridPos.X * CellSize, c.GridPos.Y * CellSize, CellSize, CellSize, col);
                    }
                }

                Raylib.DrawText($"{(int)m.Velocity.X}x{(int)m.Velocity.Y}", cell.ScreenPos.X, cell.ScreenPos.Y - CellSize, 16, Color.PINK);
            }
        }
    }
}
