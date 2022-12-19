﻿using Raylib_cs;
using System.Collections.ObjectModel;

using static PowderGame.Program;
using static PowderGame.Chunking;

namespace PowderGame
{
    public static class Cells
    {
        public static readonly int CellSize = 5;

        private static readonly List<Cell> cells = new List<Cell>();
        private static readonly Cell[,] CellsLookup = new Cell[GameW / CellSize, GameH / CellSize];
        public static readonly ReadOnlyCollection<Cell> CellsEnumerable = new ReadOnlyCollection<Cell>(cells);

        public static void CreateCells()
        {
            for (int i = 0; i < GameW / CellSize; i++)
            {
                for (int j = 0; j < GameH / CellSize; j++)
                {
                    Cell newCell = new Cell(i, j, new Materials.Void());

                    cells.Add(newCell);
                    CellsLookup[i, j] = newCell;
                }
            }
        }

        public static Cell? FindByIndex(Position index) { return FindByIndex(index.X, index.Y); }
        public static Cell? FindByIndex(int x, int y)
        {
            if (x >= CellsLookup.GetLength(0) || y >= CellsLookup.GetLength(1)) return null;
            if (x < 0 || y < 0) return null;

            return CellsLookup[x, y];
        }

        public class Cell
        {
            public Cell(int x, int y, Materials.IMaterial _material)
            {
                Index.X = x;
                Index.Y = y;

                SetMaterial(_material);
            }

            public Cell(Position index, Materials.IMaterial _material)
            {
                Index = index;

                SetMaterial(_material);
            }

            public readonly Position Index;
            public Position ScreenPos { get { return new Position(GameCorners.Left + Index.X * CellSize, GameCorners.Top + Index.Y * CellSize); } }
            public Position GridPos { get { return new Position(ScreenPos.X / CellSize, ScreenPos.Y / CellSize); } }
            public Chunk Chunk { get { return ChunksLookup[(int)Math.Floor(Index.X / Convert.ToDecimal(ChunkSize)), (int)Math.Floor(Index.Y / Convert.ToDecimal(ChunkSize))]; } }

            private Color color = Color.RED;
            public Color Color { get { return color; } }

            public Materials.IMaterial OccupyingMaterial { get; private set; } = default!;

            private void SetColor()
            {
                int r = (int)Helpers.RandomRange(OccupyingMaterial.Colors.Min.r, OccupyingMaterial.Colors.Max.r);
                int g = (int)Helpers.RandomRange(OccupyingMaterial.Colors.Min.g, OccupyingMaterial.Colors.Max.g);
                int b = (int)Helpers.RandomRange(OccupyingMaterial.Colors.Min.b, OccupyingMaterial.Colors.Max.b);
                int a = (int)Helpers.RandomRange(OccupyingMaterial.Colors.Min.a, OccupyingMaterial.Colors.Max.a);

                color = new Color(r, g, b, a);
            }

            public void Draw()
            {
                if (OccupyingMaterial.MaterialType == MaterialTypes.None) return;
                Raylib.DrawRectangle(GridPos.X * CellSize, GridPos.Y * CellSize, CellSize, CellSize, color);
            }

            public void SetMaterial(Materials.IMaterial material)
            {
                material ??= new Materials.Void();

                OccupyingMaterial = material;
                SetColor();
            }

            /// <summary> Takes the material from a cell and sets that cell's material to a given one. </summary>
            public void ReplaceMaterial(Cell replacementSource, Materials.IMaterial newMaterial)
            {
                SetMaterial(replacementSource.OccupyingMaterial);
                replacementSource.SetMaterial(newMaterial);
            }
        }

        public static bool QueryMaterial(Cell originCell, int offsetX, int offsetY, MaterialTypes materialType)
        {
            return QueryMaterial(originCell, offsetX, offsetY, new MaterialTypes[] { materialType });
        }

        public static bool QueryMaterial(Cell originCell, Position offset, MaterialTypes materialType)
        {
            return QueryMaterial(originCell, offset.X, offset.Y, new MaterialTypes[] { materialType });
        }

        public static bool QueryMaterial(Cell originCell, Position offset, MaterialTypes[] materialTypes)
        {
            return QueryMaterial(originCell, offset.X, offset.Y, materialTypes);
        }

        public static bool QueryMaterial(Cell originCell, int offsetX, int offsetY, MaterialTypes[] materialTypes)
        {
            Cell? foundCell = FindByIndex(originCell.Index.X + offsetX, originCell.Index.Y + offsetY);
            if (foundCell == null) return materialTypes.Contains(MaterialTypes.OutsideMap);

            return materialTypes.Contains(foundCell.OccupyingMaterial.MaterialType);
        }
    }
}
