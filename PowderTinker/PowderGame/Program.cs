using Raylib_cs;

using static PowderGame.Helpers;

namespace PowderGame
{
    static class Program
    {
        static readonly int WinW = 1280;
        static readonly int WinH = 720;

        public static readonly int G_GameW = 1280;
        public static readonly int G_GameH = 720;

        public static readonly int G_CellSize = 5;

        public static Cell[,] G_CellLookup = new Cell[G_GameW / G_CellSize, G_GameH / G_CellSize];
        public static List<Cell> G_Cells = new();

        public static readonly float G_PhysicsTimerTarget = .2f;
        public static readonly float G_PhysicsRate = 1f;

        public static Corners GameCorners
        {
            get
            {
                int top = (WinH / 2) - (G_GameH / 2);
                int left = (WinW / 2) - (G_GameW / 2);

                return new Corners(top, left, top + G_GameH, left + G_GameW);
            }
        }

        public static void Main()
        {
            Raylib.InitWindow(WinW, WinH, "Hello World");
            Raylib.SetTargetFPS(60);

            CreateGridSlots();

            Thread physicsThread = new Thread(new ThreadStart(PhysicsLoop));
            physicsThread.Start();

            while (!Raylib.WindowShouldClose())
            {
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.BLACK);

                Drawing.DrawCells();
                Drawing.DrawDebugContent();
                Drawing.DrawHUD();

                MouseInput.CheckMouseInput();

                Raylib.EndDrawing();
            }

            Raylib.CloseWindow();
        }

        public static void PhysicsLoop()
        {
            while (!Raylib.WindowShouldClose())
            {
                Physics.RunPhysicsOnAllCells();
            }
        }

        static void CreateGridSlots()
        {
            for (int i = 0; i < G_GameW / G_CellSize; i++)
            {
                for (int j = 0; j < G_GameH / G_CellSize; j++)
                {
                    Cell newCell = new Cell(i, j, new Materials.Void());

                    G_Cells.Add(newCell);
                    G_CellLookup[i, j] = newCell;
                }
            }
        }
    }
}