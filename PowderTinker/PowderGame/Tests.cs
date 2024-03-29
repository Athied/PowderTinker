﻿using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static PowderGame.Cells;

namespace PowderGame
{
    public static class Tests
    {
        public static void DrawCheckerboard()
        {
            for (int i = 0; i < Program.GameW; i++)
            {
                for (int j = 0; j < Program.GameH; j++)
                {
                    Color col = (i + j) % 2 == 0 ? Color.GRAY : Color.LIGHTGRAY;

                    Raylib.DrawRectangle(i * CellSize, j * CellSize, CellSize, CellSize, col);
                }
            }
        }
    }
}
