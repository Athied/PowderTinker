using Raylib_cs;
using System.Numerics;

namespace PowderGame.Materials
{
    public abstract class Powder : BaseMaterial
    {
        public sealed override MaterialTypes MaterialType { get { return MaterialTypes.Powder; } }

        // Default values for powders
        public virtual float PourSpeed { get { return 10; } }
        public override float Density { get { return 700; } }
        public override float DragResistance { get { return 25; } }

        protected override void UpdateVelocity(Cells.Cell cell)
        {
            if (cell.OccupyingMaterial != this) return;

            Physics.ApplyExternalForces(cell);

            MaterialTypes[] m = new MaterialTypes[] { MaterialTypes.Powder, MaterialTypes.Solid };

            // Basic rules:
            // 0: If there is a valid space 1 cell down, move down
            // 1: If there is a valid space 1 cell down and to the left, move there
            // 2: If there is a valid space 1 cell down and to the right, move there

            if (!Cells.QueryMaterial(cell, 0, 1, m)) return;

            if (!Cells.QueryMaterial(cell, 1, 1, m))
            {
                Velocity.AddRaw(PourSpeed, 0);
                return;
            }

            if (!Cells.QueryMaterial(cell, -1, 1, m))
            {
                Velocity.AddRaw(-PourSpeed, 0);
            }
        }
    }
}
