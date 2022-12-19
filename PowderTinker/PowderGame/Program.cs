using Raylib_cs;

namespace PowderGame
{
    static class Program
    {
        public static readonly int WinW = 1280;
        public static readonly int WinH = 720;

        public static readonly int GameW = 1280;
        public static readonly int GameH = 720;

        public static float DeltaTime { get; private set; }
        public static int FPS { get; private set; }

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

            Chunking.CreateChunks();
            Cells.CreateCells();

            Thread physicsThread = new Thread(new ThreadStart(PhysicsLoop));
            physicsThread.Start();

            if (Drawing.UseTextureRendering)
            {
                Image img = Raylib.GenImageColor(WinW, WinH, Color.BLACK);
                Raylib.ImageFormat(ref img, PixelFormat.PIXELFORMAT_UNCOMPRESSED_R8G8B8A8);
                Drawing.CellsTexture = Raylib.LoadTextureFromImage(img);
                Raylib.UnloadImage(img);
            }

            while (!Raylib.WindowShouldClose())
            {
                DeltaTime = Raylib.GetFrameTime();
                FPS = Raylib.GetFPS();

                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.BLACK);

                Drawing.DrawFrame();

                MouseInput.CheckMouseInput();
                KeyInput.CheckKeyInput();

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