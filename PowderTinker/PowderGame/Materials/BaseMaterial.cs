using Raylib_cs;
using System.Numerics;

using static PowderGame.Program;

namespace PowderGame.Materials
{
    public abstract class BaseMaterial : IMaterial
    {
        public Vector2 PhysicsTimer { get; private set; }

        public bool DrawDebugInfo { get; set; }
        public Position[] LastProjectedPath { get; set; }

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

        public void RunPhysics(Cell cell)
        {
            UpdateVelocity(cell);

            float xTime = Raylib.GetFrameTime() * G_PhysicsRate;
            float yTime = Raylib.GetFrameTime() * G_PhysicsRate;

            Vector2 addedTime = new Vector2(xTime, yTime);

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

        protected virtual void MoveToCell(Cell cell, int x, int y)
        {
            Cell? targetCell = Helpers.GetCellAtIndex(cell.Index.X + x, cell.Index.Y + y);
            if (targetCell == null) return;

            if (targetCell.OccupyingMaterial.MaterialType == MaterialTypes.None)
            {
                targetCell.ReplaceMaterial(cell, new Void());
            }

            if (targetCell.OccupyingMaterial.MaterialType == MaterialTypes.Liquid)
            {
                targetCell.ReplaceMaterial(cell, targetCell.OccupyingMaterial);
            }

            if (targetCell.OccupyingMaterial.MaterialType == MaterialTypes.Solid || targetCell.OccupyingMaterial.MaterialType == MaterialTypes.Powder)
            {
                // todo: retaining and redistributing energy
            }
        }
    }
}
