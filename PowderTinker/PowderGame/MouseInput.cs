using Raylib_cs;
using System;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using static PowderGame.Program;

namespace PowderGame
{
    static class MouseInput
    {
        private static int SelectedMaterial;
        private static readonly int MaxMaterialIndex = 3;
        public static string SelectedMaterialName { get { return GetMaterial().Name; } }

        private static int brushSize = 2;
        public static int BrushSize { get { return brushSize; } private set { brushSize = Math.Clamp(value, 1, 50); } }

        private static float brushDensity = 0.3f;
        public static float BrushDensity { get { return brushDensity; } private set { brushDensity = Math.Clamp(value, 0.1f, 1f); } }

        public static Cell? GetHoveredCell()
        {
            int mx = Raylib.GetMouseX();
            int my = Raylib.GetMouseY();

            return G_Cells.FirstOrDefault(c => mx / G_CellSize == c.GridPos.X && my / G_CellSize == c.GridPos.Y);
        }

        private static Materials.IMaterial GetMaterial()
        {
            return SelectedMaterial switch
            {
                0 => new Materials.Sand(),
                1 => new Materials.Water(),
                2 => new Materials.Void(),
                3 => new Materials.Stone(),
                _ => new Materials.Void(),
            };
        }

        public static void CheckMouseInput()
        {
            if (Raylib.GetMouseWheelMoveV().Y > 0)
            {
                if (Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_SHIFT)) BrushDensity += 0.1f;
                else BrushSize += 1;
            }
            else if (Raylib.GetMouseWheelMoveV().Y < 0)
            {
                if (Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_SHIFT)) BrushDensity -= 0.1f;
                else BrushSize -= 1;
            }

            if (Raylib.IsMouseButtonDown(MouseButton.MOUSE_BUTTON_LEFT))
            {
                Cell? cell = GetHoveredCell();
                if (cell == null) return;

                SpawnMaterialsInZone(cell);
            }
            
            if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_RIGHT))
            {
                SelectedMaterial++;

                if (SelectedMaterial > MaxMaterialIndex) SelectedMaterial = 0;
            }
        }

        public static void SpawnMaterialsInZone(Cell centralCell)
        {
            for (int i = centralCell.Index.X - BrushSize; i < centralCell.Index.X + BrushSize; i++)
            {
                for (int j = centralCell.Index.Y - BrushSize; j < centralCell.Index.Y + BrushSize; j++)
                {
                    if (Helpers.RandomRange(0, 1) > BrushDensity) continue;

                    Cell? currentCell = Helpers.GetCellAtIndex(i, j);
                    if (currentCell == null) continue;

                    Materials.IMaterial material = GetMaterial();

                    if (currentCell.OccupyingMaterial.MaterialType == MaterialTypes.None || material.MaterialType == MaterialTypes.None)
                    {
                        currentCell.SetMaterial(material);
                    }
                }
            }
        }
    }
}