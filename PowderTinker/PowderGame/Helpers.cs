namespace PowderGame
{
    public static class Helpers
    {
        private static readonly Random rand = new Random();

        public static float RandomRange(float min, float max)
        {
            return (float)rand.NextDouble() * (max - min) + min;
        }
    }
}