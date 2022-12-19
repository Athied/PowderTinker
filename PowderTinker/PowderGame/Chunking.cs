using System.Collections.ObjectModel;
using static PowderGame.Program;

namespace PowderGame
{
    public static class Chunking
    {
        public static readonly int ChunkSize = 8;

        private static readonly List<Chunk> chunks = new List<Chunk>();
        public static readonly ReadOnlyCollection<Chunk> Chunks = new ReadOnlyCollection<Chunk>(chunks);
        public static readonly Chunk[,] ChunksLookup = new Chunk[GameW / ChunkSize, GameH / ChunkSize];

        public static void CreateChunks()
        {
            for (int i = 0; i < GameW / ChunkSize; i++)
            {
                for (int j = 0; j < GameH / ChunkSize; j++)
                {
                    int x = i * ChunkSize;
                    int y = j * ChunkSize;

                    Chunk chunk = new Chunk(x, y, i, j);

                    chunks.Add(chunk);
                    ChunksLookup[i, j] = chunk;
                }
            }
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

            public Position ScreenPos { get { return new Position(GameCorners.Left + TopLeftCellIndex.X * Cells.CellSize, GameCorners.Top + TopLeftCellIndex.Y * Cells.CellSize); } }

            public bool Sleeping = false;
        }
    }
}
