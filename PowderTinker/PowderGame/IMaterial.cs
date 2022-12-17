using Raylib_cs;
using System.Numerics;

namespace PowderGame
{
    public interface IMaterial
    {
        float PhysicsTimer { get; set; }

        MaterialTypes MaterialType { get; }

        string Name { get; }

        ColorRange Colors { get; }

        float OverallSpeed { get; }

        Vector2 Velocity { get; set; }
    }
}
