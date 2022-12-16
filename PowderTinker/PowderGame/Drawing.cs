using Raylib_cs;
using System.Text;
using static PowderGame.Program;

namespace PowderGame
{
    public static class Drawing
    {
        private static readonly int FontSize = 18;

        public static void DrawDebugContent()
        {
            //DrawGrid();
            DrawBorder();
        }

        public static void DrawHUD()
        {
            Cell? hoveredCell = MouseInput.GetHoveredCell();

            string cellPos = hoveredCell != null ? $"Cell: {hoveredCell.Material.Name} ({hoveredCell.IndexX}x{hoveredCell.IndexY})" : "Cell: unknown";
            string selectedMat = $"Using: {MouseInput.SelectedMaterialName}";
            string brushSize = $"Brush size: {MouseInput.BrushSize}";
            string brushDensity = $"Brush density: {MouseInput.BrushDensity:n1}";
            string physicsTime = $"Physics Time (ms): {Physics.AveragePhysicsTimeTaken:n2}";
            string activeCells = $"Active Cells: {Physics.ActiveCells}";
            string sand = $"Cells (Sand): {G_Cells.Where(c => c.Material.Name == "Sand").Count()}";
            string water = $"Cells (Water): {G_Cells.Where(c => c.Material.Name == "Water").Count()}";

            StringBuilder sb = new StringBuilder();
            sb.Append(cellPos + "\n");
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
            foreach (Cell cell in G_Cells)
            {
                cell.Draw();
            }
        }

        static void DrawBorder()
        {
            Raylib.DrawRectangleLines(GameCorners.Left, GameCorners.Top, G_GameW, G_GameH, Color.RED);
        }

        static void DrawGrid()
        {
            // Columns
            for (int columnPos = GameCorners.Left + G_CellSize; columnPos < GameCorners.Right; columnPos += G_CellSize)
            {
                Color col = columnPos % 4 == 0 ? Color.BLUE : Color.DARKBLUE;

                Raylib.DrawLine(columnPos, GameCorners.Top, columnPos, GameCorners.Bottom, col);
            }

            // Rows
            for (int rowPos = GameCorners.Top + G_CellSize; rowPos < GameCorners.Bottom; rowPos += G_CellSize)
            {
                Color col = rowPos % 4 == 0 ? Color.BLUE : Color.DARKBLUE;

                Raylib.DrawLine(GameCorners.Left, rowPos, GameCorners.Right, rowPos, col);
            }
        }
    }
}
