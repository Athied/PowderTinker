using Raylib_cs;

namespace PowderGame
{
    static class Program
    {
        static readonly int WinW = 1280;
        static readonly int WinH = 720;

        public static readonly int GameW = 1280;
        public static readonly int GameH = 720;

        public static Corners GameCorners
        {
            get
            {
                int top = (WinH / 2) - (GameH / 2);
                int left = (WinW / 2) - (GameW / 2);

                return new Corners(top, left, top + GameH, left + GameW);
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