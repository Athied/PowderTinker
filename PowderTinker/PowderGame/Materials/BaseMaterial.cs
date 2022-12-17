using Raylib_cs;
using System.Numerics;

using static PowderGame.Program;

namespace PowderGame.Materials
{
    public abstract class BaseMaterial : IMaterial
    {
        public float PhysicsTimer { get; private set; }

        public void RunPhysicsOnTimer(Cell cell)
        {
            PhysicsTimer += Raylib.GetFrameTime() * G_PhysicsRate * OverallSpeed;

            if (PhysicsTimer > G_PhysicsTimerTarget)
            {
                PhysicsTimer = 0;

                RunPhysics(cell);
            }
        }

        public virtual void RunPhysics(Cell cell) { }

        public Vector2 Velocity { get; set; }

        public virtual MaterialTypes MaterialType { get { return MaterialTypes.None; } }

        public virtual string Name { get { return "None"; } }
        public virtual ColorRange Colors { get { return new ColorRange(Color.BLACK, Color.BLACK); } }

        public virtual float OverallSpeed { get { return 2; } }
    }
}
