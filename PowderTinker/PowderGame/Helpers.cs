using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowderGame
{
    public static class Helpers
    {
        private static readonly Random rand = new Random();

        public static float RandomRange(float min, float max)
        {
            return (float)rand.NextDouble() * (max - min) + min;
        }

        public static Cell? GetCellAtIndex(int x, int y)
        {
            if (x >= Program.G_CellLookup.GetLength(0) || y >= Program.G_CellLookup.GetLength(1)) return null;
            if (x < 0 || y < 0) return null;

            return Program.G_CellLookup[x, y];
        }
    }
}