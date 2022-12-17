using Raylib_cs;
using System.Numerics;

namespace PowderGame
{
    public abstract class BaseMaterial : IMaterial
    {
        public float PhysicsTimer { get; set; }

        public Vector2 Velocity { get; set; }

        public virtual MaterialTypes MaterialType { get { return MaterialTypes.None; } }

        public virtual string Name { get { return "None"; } }
        public virtual ColorRange Colors { get { return new ColorRange(Color.BLACK, Color.BLACK); } }

        public virtual float OverallSpeed { get { return 2; } }

        public virtual void Physics() { }
    }
}
