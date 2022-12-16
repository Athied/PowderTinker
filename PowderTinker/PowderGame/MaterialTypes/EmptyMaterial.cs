using Raylib_cs;

namespace PowderGame
{
    public abstract class BaseMaterial : IMaterial
    {
        public float PhysicsTimer { get; set; }

        public virtual MaterialTypes MaterialType { get { return MaterialTypes.None; } }

        public virtual string Name { get { return "None"; } }
        public virtual ColorRange Colors { get { return new ColorRange(Color.BLACK, Color.BLACK); } }

        public virtual float OverallSpeed { get { return 2; } }
    }

    public class EmptyMaterial : BaseMaterial
    {
        public override MaterialTypes MaterialType { get { return MaterialTypes.None; } }

        public override string Name { get { return "None"; } }
        public override ColorRange Colors { get { return new ColorRange(Color.BLACK, Color.BLACK); } }

        public override float OverallSpeed { get { return 0; } }
    }
}
