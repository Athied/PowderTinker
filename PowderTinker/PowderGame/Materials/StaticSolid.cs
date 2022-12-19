namespace PowderGame.Materials
{
    public abstract class StaticSolid : BaseMaterial
    {
        public sealed override MaterialTypes MaterialType { get { return MaterialTypes.Solid; } }

        protected override void UpdateVelocity(Cells.Cell cell)
        {
            base.UpdateVelocity(cell);
        }
    }
}
