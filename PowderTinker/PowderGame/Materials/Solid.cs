namespace PowderGame.Materials
{
    public abstract class NonStaticSolid : BaseMaterial
    {
        public sealed override MaterialTypes MaterialType { get { return MaterialTypes.Solid; } }

        public override void Physics()
        {
            base.Physics();
        }
    }
}