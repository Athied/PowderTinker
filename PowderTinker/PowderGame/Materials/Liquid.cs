using Raylib_cs;

namespace PowderGame.Materials
{
    public abstract class Liquid : BaseMaterial
    {
        public sealed override MaterialTypes MaterialType { get { return MaterialTypes.Liquid; } }

        // Default values for liquids
        public virtual float PourSpeed { get { return 25; } }
        public virtual float DispersionSpeed { get { return 1500; } }
        public override float Density { get { return 500; } }
        public override float DragResistance { get { return 25; } }

        protected override void UpdateVelocity(Cell cell)
        {
            if (cell.OccupyingMaterial != this) return;

            Physics.ApplyExternalForces(cell);

            // Basic Rules:
            // 0: If there is a valid space 1 cell down, move down
            // 1: If there is a valid space 1 cell down and to the left, move there
            // 2: If there is a valid space 1 cell down and to the right, move there
            // 3: If there is a valid space 1 cell to the left and right, stop.

            float xDragForce = Velocity.X * (Physics.ExternalForces.AirDensity / (DragResistance / 4));

            if (Helpers.CheckForMaterialsRelative(cell, 0, 1, new MaterialTypes[] { MaterialTypes.None, MaterialTypes.OutsideMap }))
            {
                Velocity.Reduce(xDragForce, 0);
                return;
            }

            bool downLeftEmpty = Helpers.CheckForMaterialsRelative(cell, -1, 1, MaterialTypes.None);
            bool downRightEmpty = Helpers.CheckForMaterialsRelative(cell, 1, 1, MaterialTypes.None);

            if (downLeftEmpty && downRightEmpty)
            {
                Velocity.Reduce(xDragForce, 0);
                return;
            }

            if (downLeftEmpty)
            {
                Velocity.AddRaw(-PourSpeed, 0);
                return;
            }

            if (downRightEmpty)
            {
                Velocity.AddRaw(PourSpeed, 0);
                return;
            }

            bool leftEmpty = Helpers.CheckForMaterialsRelative(cell, -1, 0, MaterialTypes.None);
            bool rightEmpty = Helpers.CheckForMaterialsRelative(cell, 1, 0, MaterialTypes.None);

            if (leftEmpty && rightEmpty)
            {
                Velocity.Reduce(xDragForce, 0);
                return;
            }

            if (leftEmpty)
            {
                if (Velocity.X > 0) Velocity = new Force(0, 0);
                Velocity.AddRaw(-DispersionSpeed, 0);
            }

            if (rightEmpty)
            {
                if (Velocity.X < 0) Velocity = new Force(0, 0);
                Velocity.AddRaw(DispersionSpeed, 0);
            }
        }
    }
}