namespace PowderGame.Materials
{
    public abstract class Liquid : BaseMaterial
    {
        public sealed override MaterialTypes MaterialType { get { return MaterialTypes.Liquid; } }

        public override void Physics()
        {
            base.Physics();
        }
    }
}