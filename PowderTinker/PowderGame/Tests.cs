using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowderGame
{
    public static class Tests
    {
        public static void DrawCheckerboard()
        {
            for (int i = 0; i < Program.G_GameW; i++)
            {
                for (int j = 0; j < Program.G_GameH; j++)
                {
                    Color col = (i + j) % 2 == 0 ? Color.GRAY : Color.LIGHTGRAY;

                    Raylib.DrawRectangle(i * Program.G_CellSize, j * Program.G_CellSize, Program.G_CellSize, Program.G_CellSize, col);
                }
            }
        }
    }
}
