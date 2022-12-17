using Raylib_cs;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;

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

    public struct MaterialTarget
    {
        public MaterialTarget(Vector2 desiredIndex, MaterialTypes[] desiredMaterialTypes)
        {
            DesiredIndex = desiredIndex;
            DesiredMaterialTypes = desiredMaterialTypes;
        }

        public Vector2 DesiredIndex;
        public MaterialTypes[] DesiredMaterialTypes;
    }
}