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

            return G_Cells.FirstOrDefault(c => mx / G_CellSize == c.GridX && my / G_CellSize == c.GridY);
        }

        private static IMaterial GetMaterial()
        {
            switch (SelectedMaterial)
            {
                case 0: return new Sand();
                case 1: return new Water();
                case 2: return new EmptyMaterial();
                case 3: return new Stone();
            }

            return new EmptyMaterial();
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
            for (int i = centralCell.IndexX - BrushSize; i < centralCell.IndexX + BrushSize; i++)
            {
                for (int j = centralCell.IndexY - BrushSize; j < centralCell.IndexY + BrushSize; j++)
                {
                    // 1 in 3 chance of spawning the material here

                    if (Helpers.RandomRange(0, 1) > BrushDensity) continue;



                    //if (Raylib.GetRandomValue(0, 1) != 0) continue;

                    Cell? currentCell = Helpers.GetCellAtIndex(i, j);
                    if (currentCell == null) continue;

                    IMaterial material = GetMaterial();

                    if (currentCell.Material.MaterialType == MaterialTypes.None || material.MaterialType == MaterialTypes.None)
                    {
                        currentCell.Material = material;
                    }
                }
            }
        }
    }
}