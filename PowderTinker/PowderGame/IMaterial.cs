using Raylib_cs;
using System.Numerics;

namespace PowderGame.Materials
{
    public interface IMaterial
    {
        MaterialTypes MaterialType { get; }

        string Name { get; }

        ColorRange Colors { get; }

        float OverallSpeed { get; }

        Vector2 Velocity { get; set; }

        float PhysicsTimer { get; }
        void RunPhysicsOnTimer(Cell cell);
        void RunPhysics(Cell cell);
    }
}
