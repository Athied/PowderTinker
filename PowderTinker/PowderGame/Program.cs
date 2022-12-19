using Raylib_cs;

using static PowderGame.Cells;

namespace PowderGame
{
    static class Program
    {
        static readonly int WinW = 1280;
        static readonly int WinH = 720;

        public static readonly int G_GameW = 1280;
        public static readonly int G_GameH = 720;

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

            Cells.CreateCells();

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
    }
}