namespace PowderGame.Materials
{
    public abstract class NonStaticSolid : BaseMaterial
    {
        public sealed override MaterialTypes MaterialType { get { return MaterialTypes.Solid; } }

        protected override void UpdateVelocity(Cells.Cell cell)
        {
            if (cell.OccupyingMaterial != this) return;

            // Rules:
            // A: Solids will displace liquids rather than sit on top of them
            // B: Solids will displace powders rather than sit on top of them
            // 0: If there is a valid space n cells down, move down

            MaterialTypes[] validTypes = new MaterialTypes[] { MaterialTypes.None, MaterialTypes.Liquid, MaterialTypes.Powder };

            CellMovement.TryMoveAlongPath(cell, validTypes, true, new Position[]
            {
                new (0, 1)
            });
        }
    }
}