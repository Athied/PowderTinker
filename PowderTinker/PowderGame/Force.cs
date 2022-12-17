using System.Numerics;

namespace PowderGame
{
    public class Force
    {
        public Force(float x, float y)
        {
            X = x;
            Y = y;
        }

        public float X;
        public float Y;

        public Force Product(Vector2 other)
        {
            return new Force(X * other.X, Y * other.Y);
        }

        public Force Product(Force other)
        {
            return new Force(X * other.X, Y * other.Y);
        }

        public Force Product(float other)
        {
            return new Force(X * other, Y * other);
        }

        public void AddRaw(Vector2 f) { AddRaw(f.X, f.Y); }
        public void AddRaw(Force f) { AddRaw(f.X, f.Y); }
        public void AddRaw(float x, float y)
        {
            X += x;
            Y += y;
        }

        public void Increase(Vector2 amount) { Increase(amount.X, amount.Y); }
        public void Increase(Force amount) { Increase(amount.X, amount.Y); }

        public void Increase(float x, float y)
        {
            int xMod = X > 0 ? 1 : -1;
            int yMod = Y > 0 ? 1 : -1;

            X += (Math.Abs(x) * xMod);
            Y += (Math.Abs(y) * yMod);
        }

        public void Reduce(Vector2 amount) { Reduce(amount.X, amount.Y); }
        public void Reduce(Force amount) { Reduce(amount.X, amount.Y); }
        public void Reduce(float x, float y)
        {
            int xMod = X > 0 ? -1 : 1;
            int yMod = Y > 0 ? -1 : 1;

            if (x != 0) X += (Math.Abs(x) * xMod);
            if (y != 0) Y += (Math.Abs(y) * yMod);

            int xModNew = X > 0 ? -1 : 1;
            int yModNew = Y > 0 ? -1 : 1;

            if (xMod != xModNew) X = 0;
            if (yMod != yModNew) Y = 0;
        }
    }
}
