using System.Numerics;
using static PowderGame.Program;

namespace PowderGame
{
    public static class Physics
    {
        public static class ExternalForces
        {
            public static Vector2 Gravity = new Vector2(0, 9.8f);
        }

        private static readonly List<float> AvgMilliseconds = new List<float>();

        public static float AveragePhysicsTimeTaken { get; private set; }
        public static int ActiveCells { get; private set; }

        public static void RunPhysicsOnAllCells()
        {
            DateTime t = DateTime.Now;

            IEnumerable<Cell> cells = G_Cells.Where(c => c.OccupyingMaterial.MaterialType != MaterialTypes.None);

            ActiveCells = cells.Count();

            Parallel.ForEach(G_Cells, cell =>
            {
                cell.OccupyingMaterial.RunPhysicsOnTimer(cell);
            });

            if (AvgMilliseconds.Count > 180) AvgMilliseconds.RemoveAt(0);

            AvgMilliseconds.Add((float)(DateTime.Now - t).TotalMilliseconds);

            AveragePhysicsTimeTaken = (float)AvgMilliseconds.Average();
        }
    }
}
