using PowderGame.Materials;
using Raylib_cs;
using System.Text;

using static PowderGame.Program;
using static PowderGame.Cells;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Data;
using System.Security.Principal;

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
        private static readonly Color AwakeChunksColor = new Color(0, 128, 128, 255);
        private static readonly Color CellVelocityColor = Color.PINK;

        public static bool ShowChunks = false;
        public static bool ShowGrid = false;
        public static bool ShowBorder = false;

        private static readonly List<float> AvgMilliseconds = new List<float>();
        public static float AverageDrawingTimeTaken { get; private set; }

        private static byte[] CellsPixelBuffer = new byte[WinW * WinH * 4];
        public static Texture2D CellsTexture = new Texture2D() { format = PixelFormat.PIXELFORMAT_UNCOMPRESSED_R8G8B8A8, width = WinW, height = WinH };

        public static readonly bool UseTextureRendering = true;

        public static void DrawFrame()
        {
            DateTime t = DateTime.Now;

            DrawCells();
            DrawDebugContent();
            DrawHUD();

            if (AvgMilliseconds.Count > 180) AvgMilliseconds.RemoveAt(0);
            AvgMilliseconds.Add((float)(DateTime.Now - t).TotalMilliseconds);
            AverageDrawingTimeTaken = (float)AvgMilliseconds.Average();

            //Raylib.TraceLog(TraceLogLevel.LOG_INFO, AverageDrawingTimeTaken.ToString());
        }

        public static void SetPixelColor(int x, int y, byte r, byte g, byte b, byte a)
        {
            int index = (x + y * GameW) * 4;

            CellsPixelBuffer[index] = r;

            CellsPixelBuffer[index + 1] = g;

            CellsPixelBuffer[index + 2] = b;

            CellsPixelBuffer[index + 3] = a;
        }

        public static void DrawCells()
        {
            if (UseTextureRendering)
            {
                unsafe
                {
                    GCHandle pinned = GCHandle.Alloc(CellsPixelBuffer, GCHandleType.Pinned);
                    Raylib.UpdateTexture(CellsTexture, pinned.AddrOfPinnedObject().ToPointer());
                    pinned.Free();
                }

                Raylib.DrawTexture(CellsTexture, 0, 0, Color.WHITE);
            }
            else
            {
                foreach (Cell cell in CellsEnumerable)
                {
                    cell.DrawAsRect();
                }
            }
        }

        public static void DrawDebugContent()
        {
            if (ShowGrid) DrawGrid();
            if (ShowChunks) DrawChunksDebug();
            //DrawCellDebug();
            if (ShowBorder) DrawBorder();
        }

        public static void DrawHUD()
        {
            //Cell? hoveredCell = MouseInput.GetHoveredCell();

            //string cell = hoveredCell != null ? $"Cell: {hoveredCell.OccupyingMaterial.Name} ({hoveredCell.Index.X}x{hoveredCell.Index.Y})" : "Cell: unknown";
            //string chunk = hoveredCell != null ? $"{hoveredCell.Chunk.ChunkIndex.X}x{hoveredCell.Chunk.ChunkIndex.Y}" : "no chunk";
            string selectedMat = $"Using: {MouseInput.SelectedMaterialName}";
            string brushSize = $"Brush size: {MouseInput.BrushSize}";
            string brushDensity = $"Brush density: {MouseInput.BrushDensity:n1}";
            string physicsTime = $"Physics Time (ms): {Physics.AveragePhysicsTimeTaken:n2}";
            string drawTime = $"Draw Time (ms): {AverageDrawingTimeTaken:n2}";
            string activeCells = $"Active Cells: {Physics.ActiveCells}";
            //string sand = $"Cells (Sand): {CellsEnumerable.Where(c => c.OccupyingMaterial.Name == "Sand").Count()}";
            //string water = $"Cells (Water): {CellsEnumerable.Where(c => c.OccupyingMaterial.Name == "Water").Count()}";

            StringBuilder sb = new StringBuilder();
            //sb.Append(cell + $" ({chunk})" + "\n");
            sb.Append(selectedMat + "\n");
            sb.Append(brushSize + "\n");
            sb.Append(brushDensity + "\n");
            sb.Append(physicsTime + "\n");
            sb.Append(drawTime + "\n");
            sb.Append(activeCells + "\n\n");
            //sb.Append(sand + "\n");
            //sb.Append(water + "\n");

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
