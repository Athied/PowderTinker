using System.Collections.ObjectModel;

namespace PowderGame
{
    public static class Chunking
    {
        private static readonly List<Chunk> chunks = new List<Chunk>();
        public static readonly ReadOnlyCollection<Chunk> Chunks = new ReadOnlyCollection<Chunk>(chunks);

        public static readonly int ChunkSize = 64;

        public class Chunk
        {
            public Chunk(Position rootIndex)
            {
                RootIndex = rootIndex;

                chunks.Add(this);
            }

            public readonly Position RootIndex;

            public bool Sleeping = false;
        }
    }
}
