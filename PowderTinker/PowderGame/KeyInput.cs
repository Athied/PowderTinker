using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PowderGame.Cells;

namespace PowderGame
{
    static class KeyInput
    {

        public static void CheckKeyInput()
        {
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_SPACE))
            {
                Cell? cell = MouseInput.GetHoveredCell();
                if (cell == null) return;

                cell.OccupyingMaterial.DrawDebugInfo = !cell.OccupyingMaterial.DrawDebugInfo;
            }

            if (Raylib.IsKeyPressed(KeyboardKey.KEY_ONE))
            {
                Drawing.ShowChunks = !Drawing.ShowChunks;
            }

            if (Raylib.IsKeyPressed(KeyboardKey.KEY_TWO))
            {
                Drawing.ShowGrid = !Drawing.ShowGrid;
            }

            if (Raylib.IsKeyPressed(KeyboardKey.KEY_THREE))
            {
                Drawing.ShowBorder = !Drawing.ShowBorder;
            }
        }
    }
}
