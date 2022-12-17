namespace PowderGame.Materials
{
    public abstract class Powder : BaseMaterial
    {
        public sealed override MaterialTypes MaterialType { get { return MaterialTypes.Powder; } }

        public override void Physics()
        {
            base.Physics();
        }
    }
}
