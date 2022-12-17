using Raylib_cs;

namespace PowderGame
{
    public struct Corners
    {
        public Corners(int top, int left, int bottom, int right)
        {
            Top = top;
            Left = left;
            Bottom = bottom;
            Right = right;
        }

        public int Top;
        public int Left;
        public int Bottom;
        public int Right;
    }

    public struct ColorRange
    {
        public ColorRange(Color min, Color max)
        {
            Min = min;
            Max = max;
        }

        public Color Min;
        public Color Max;
    }

    public struct Position
    {
        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X;
        public int Y;
    }
}