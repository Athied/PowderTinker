using Raylib_cs;
using System.Numerics;

namespace PowderGame
{
    [Obsolete(message: "Left in for reference and potential niche purposes. \nPlease move the cells by managing their velocity in their respective UpdateVelocity overrides.")]
    static class CellMovement
    {
        [Obsolete]
        public static bool TryMoveRelative(Cell cell, MaterialTypes[] validTypes, Position offset, bool swapCells)
        {
            Position moveToPos = new Position(cell.Index.X + offset.X, cell.Index.Y + offset.Y);
            return TryMove(cell, validTypes, moveToPos, swapCells);
        }

        [Obsolete]
        public static int TryMoveAlongPath(Cell cell, MaterialTypes[] validTypes, bool swapCells, Position[] pathPoints)
        {
            int steps = 0;

            foreach (Position offset in pathPoints)
            {
                Position moveToPos = new Position(cell.Index.X + offset.X, cell.Index.Y + offset.Y);
                if (TryMove(cell, validTypes, moveToPos, swapCells)) return steps;

                steps++;
            }

            return -1;
        }

        [Obsolete]
        public static bool TryMove(Cell cell, MaterialTypes[] validTypes, Position index, bool swapCells)
        {
            Position[] path = FindOptimalPath(cell.Index, index);
            cell.OccupyingMaterial.LastProjectedPath = path;

            if (path.Length == 0)
            {
                Cell? target = Helpers.GetCellAtIndex(index.X, index.Y);
                if (target == null) return false;

                Move(cell, target, swapCells);
                return true;
            }

            int steps = -1;
            for (int i = 0; i < path.Length; i++)
            {
                Cell? target = Helpers.GetCellAtIndex(path[i].X, path[i].Y);
                if (target == null) continue;

                if (validTypes.Contains(target.OccupyingMaterial.MaterialType))
                {
                    steps++;

                    Move(cell, target, swapCells);
                }
            }

            return steps != -1;
        }

        [Obsolete]
        private static void Move(Cell from, Cell to, bool swapCells)
        {
            if (!swapCells) to.ReplaceMaterial(from, new Materials.Void());
            else to.ReplaceMaterial(from, to.OccupyingMaterial);
        }

        [Obsolete]
        public static Position[] FindOptimalPath(Position p1, Position p2)
        {
            if (p1.Equals(p2)) return new Position[] { p2 };

            int xDiff = p1.X - p2.X;
            int yDiff = p1.Y - p2.Y;

            bool xDiffLarger = Math.Abs(xDiff) > Math.Abs(yDiff);

            int xMod = xDiff < 0 ? 1 : -1;
            int yMod = yDiff < 0 ? 1 : -1;

            int longerSideLen = Math.Max(Math.Abs(xDiff), Math.Abs(yDiff));
            int shorterSideLen = Math.Min(Math.Abs(xDiff), Math.Abs(yDiff));

            float slope = (shorterSideLen == 0 || longerSideLen == 0) ? 0 : ((float)shorterSideLen / longerSideLen);

            int shorterSideInc;

            List<Position> path = new List<Position>();

            for (int i = 1; i <= longerSideLen; i++)
            {
                shorterSideInc = (int)Math.Round(i * slope);

                int xInc = xDiffLarger ? i : shorterSideInc;
                int yInc = xDiffLarger ? shorterSideInc : i;

                int currentX = p1.X + (xInc * xMod);
                int currentY = p1.Y + (yInc * yMod);

                path.Add(new Position(currentX, currentY));
            }

            return path.ToArray();
        }
    }
}
