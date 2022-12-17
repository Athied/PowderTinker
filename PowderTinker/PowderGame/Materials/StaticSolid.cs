namespace PowderGame.Materials
{
    public abstract class StaticSolid : BaseMaterial
    {
        public sealed override MaterialTypes MaterialType { get { return MaterialTypes.Solid; } }

        public override void RunPhysics(Cell cell)
        {
            base.RunPhysics(cell);
        }
    }
}
