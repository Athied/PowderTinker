using Raylib_cs;

namespace PowderGame
{
    public interface IMaterial
    {
        float PhysicsTimer { get; set; }

        MaterialTypes MaterialType { get; }

        string Name { get; }

        ColorRange Colors { get; }

        float OverallSpeed { get; }
    }
}
