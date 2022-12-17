namespace PowderGame.Materials
{
    public abstract class Powder : BaseMaterial
    {
        public sealed override MaterialTypes MaterialType { get { return MaterialTypes.Powder; } }

        public override void RunPhysics(Cell cell)
        {
            if (cell.OccupyingMaterial != this) return;

            // Rules:
            // A: Powders will displace liquids rather than sit on top of them
            // 0: If there is a valid space n cells down, move down
            // 1: If there is a valid space n cells down and to the left, move there
            // 2: If there is a valid space n cells down and to the right, move there

            MaterialTypes[] validTypes = new MaterialTypes[] { MaterialTypes.None, MaterialTypes.Liquid };

            CellMovement.TryMoveAlongPath(cell, validTypes, true, new (int, int)[]
            {
                (0, 1),
                (-1, 1),
                (1, 1)
            });
        }
    }
}
