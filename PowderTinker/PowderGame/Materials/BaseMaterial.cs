using Raylib_cs;
using System.Net.Security;
using System.Numerics;

using static PowderGame.Program;
using static PowderGame.Cells;

namespace PowderGame.Materials
{
    public abstract class BaseMaterial : IMaterial
    {
        public Vector2 PhysicsTimer { get; private set; }

        public bool DrawDebugInfo { get; set; }

        [Obsolete("Only set by deprecated methods")]
        public Position[]? LastProjectedPath { get; set; }

        public virtual float DragResistance { get; }
        public virtual float Density { get; }

        public Force TerminalVelocity { get; set; } = new Force(0, 0);
        public virtual Force Velocity { get; set; } = new Force(0, 0);
        public virtual Force Drag { get; set; } = new Force(0, 0);

        protected virtual void UpdateVelocity(Cell cell) { }

        public virtual MaterialTypes MaterialType { get { return MaterialTypes.None; } }

        public virtual string Name { get { return "None"; } }
        public virtual ColorRange Colors { get { return new ColorRange(Color.BLACK, Color.BLACK); } }

        public virtual float OverallSpeed { get { return 2; } }
        public bool KillNextFrame { get; protected set; }

        public void RunPhysics(Cell cell)
        {
            if (KillNextFrame)
            {
                cell.SetMaterial(new Void());
                return;
            }

            UpdateVelocity(cell);

            float timeIncrement = Raylib.GetFrameTime() * Physics.PhysicsSpeedMult;
            Vector2 addedTime = new Vector2(timeIncrement, timeIncrement);
            PhysicsTimer += addedTime;

            if (PhysicsTimer.X > Raylib.GetFPS() / Math.Abs(Velocity.X))
            {
                PhysicsTimer = new Vector2(0, PhysicsTimer.Y);

                int x = 0;
                if (Velocity.X != 0) x = Velocity.X > 0 ? 1 : -1;

                MoveToCell(cell, x, 0);
            }

            if (PhysicsTimer.Y > Raylib.GetFPS() / Math.Abs(Velocity.Y))
            {
                PhysicsTimer = new Vector2(PhysicsTimer.X, 0);

                int y = 0;
                if (Velocity.Y != 0) y = Velocity.Y > 0 ? 1 : -1;

                MoveToCell(cell, 0, y);
            }
        }

        protected virtual bool MoveToCell(Cell cell, int x, int y)
        {
            Cell? targetCell = Cells.FindByIndex(cell.Index.X + x, cell.Index.Y + y);
            if (targetCell == null) return false;

            if (MaterialType == MaterialTypes.Solid || MaterialType == MaterialTypes.Powder)
            {
                if (targetCell.OccupyingMaterial.MaterialType == MaterialTypes.None)
                {
                    targetCell.ReplaceMaterial(cell, new Void());
                    return true;
                }

                if (targetCell.OccupyingMaterial.MaterialType == MaterialTypes.Liquid)
                {
                    targetCell.ReplaceMaterial(cell, targetCell.OccupyingMaterial);
                    return true;
                }
            }
            else if (MaterialType == MaterialTypes.Liquid)
            {
                if (targetCell.OccupyingMaterial.MaterialType == MaterialTypes.None)
                {
                    targetCell.ReplaceMaterial(cell, new Void());
                    return true;
                }
            }

            return false;
        }
    }
}
