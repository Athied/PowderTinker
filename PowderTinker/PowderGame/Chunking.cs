using System.Collections.ObjectModel;

using static PowderGame.Program;
using static PowderGame.Cells;

namespace PowderGame
{
    public static class Chunking
    {
        public static readonly int ChunkSize = 8;

        private static readonly List<Chunk> chunks = new List<Chunk>();
        private static readonly Chunk[,] ChunksLookup = new Chunk[GameW / (ChunkSize * CellSize) + 1, GameH / (ChunkSize * CellSize) + 1];
        public static readonly ReadOnlyCollection<Chunk> Chunks = new ReadOnlyCollection<Chunk>(chunks);

        public static void CreateChunks()
        {
            for (int i = 0; i < GameW / (ChunkSize * CellSize) + 1; i++)
            {
                for (int j = 0; j < GameH / (ChunkSize * CellSize) + 1; j++)
                {
                    int x = i * ChunkSize;
                    int y = j * ChunkSize;

                    Chunk chunk = new Chunk(x, y, i, j);

                    chunks.Add(chunk);
                    ChunksLookup[i, j] = chunk;
                }
            }
        }

        public static Chunk? FindByIndex(Position index) { return FindByIndex(index.X, index.Y); }
        public static Chunk? FindByIndex(int x, int y)
        {
            if (x >= ChunksLookup.GetLength(0) || y >= ChunksLookup.GetLength(1)) return null;
            if (x < 0 || y < 0) return null;

            return ChunksLookup[x, y];
        }

        public static Chunk? FindByCellIndex(Position index) { return FindByCellIndex(index.X, index.Y); }
        public static Chunk? FindByCellIndex(int x, int y)
        {
            // slow
            return FindByIndex((int)Math.Floor(x / Convert.ToDecimal(ChunkSize)), (int)Math.Floor(y / Convert.ToDecimal(ChunkSize)));
        }

        public class Chunk
        {
            public Chunk(int topLeftCellIndexX, int topLeftCellIndexY, int chunkIndexX, int chunkIndexY)
            {
                TopLeftCellIndex = new Position(topLeftCellIndexX, topLeftCellIndexY);
                ChunkIndex = new Position(chunkIndexX, chunkIndexY);
            }

            public Chunk(Position topLeftCellIndex, Position chunkIndex)
            {
                TopLeftCellIndex = topLeftCellIndex;
                ChunkIndex = chunkIndex;
            }

            public readonly Position TopLeftCellIndex;
            public readonly Position ChunkIndex;

            public Position ScreenPos { get { return new Position(GameCorners.Left + TopLeftCellIndex.X * CellSize, GameCorners.Top + TopLeftCellIndex.Y * CellSize); } }

            public bool Sleeping { get; private set; }
            private bool SleepNextFrame;

            public void SetSleepNextFrame(bool sleepNextFrame)
            {
                SleepNextFrame = sleepNextFrame;
            }

            public void UpdateSleepState()
            {
                Sleeping = SleepNextFrame;
                SleepNextFrame = true;
            }

            private Cell[]? containedCells = null;
            public Cell[] ContainedCells
            {
                get
                {
                    if (containedCells == null)
                    {
                        List<Cell> cells = new List<Cell>();

                        for (int i = 0; i < ChunkSize; i++)
                        {
                            for (int j = 0; j < ChunkSize; j++)
                            {
                                Cell? cell = Cells.FindByIndex(TopLeftCellIndex.X + i, TopLeftCellIndex.Y + j);
                                if (cell == null) continue;

                                cells.Add(cell);
                            }
                        }

                        containedCells = cells.ToArray();
                    }

                    return containedCells;
                }
            }
        }
    }
}
