using Raylib_cs;
using System.Numerics;

namespace PowderGame.Materials
{
    public abstract class Powder : BaseMaterial
    {
        public sealed override MaterialTypes MaterialType { get { return MaterialTypes.Powder; } }

        protected override void UpdateVelocity(Cell cell)
        {
            if (cell.OccupyingMaterial != this) return;

            Physics.ApplyExternalForces(cell);

            MaterialTypes[] m = new MaterialTypes[] { MaterialTypes.Powder, MaterialTypes.Solid };

            if (!Helpers.CheckForMaterialsRelative(cell, 0, 1, m)) return;

            if (!Helpers.CheckForMaterialsRelative(cell, 1, 1, m))
            {
                Velocity.AddRaw(10, 0);
                return;
            }

            if (!Helpers.CheckForMaterialsRelative(cell, -1, 1, m))
            {
                Velocity.AddRaw(-10, 0);
            }

            //float x = Velocity.Y / 5;
            //float y = x > 0 ? x : 0;

            //Velocity = new Vector2(x, y);

            // Rules:
            // A: Powders will displace liquids rather than sit on top of them
            // 0: If there is a valid space n cells down, move down
            // 1: If there is a valid space n cells down and to the left, move there
            // 2: If there is a valid space n cells down and to the right, move there

            //MaterialTypes[] validTypes = new MaterialTypes[] { MaterialTypes.None, MaterialTypes.Liquid };

            //CellMovement.TryMoveAlongPath(cell, validTypes, true, new Position[]
            //{
            //    new (0, 1),
            //    new (-1, 1),
            //    new (1, 1)
            //});
        }
    }
}
