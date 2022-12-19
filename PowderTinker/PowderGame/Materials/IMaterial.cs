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

        float Density { get; }
        float DragResistance { get; }

        Force TerminalVelocity { get; set; }
        Force Velocity { get; set; }
        Force Drag { get; set; }

        Vector2 PhysicsTimer { get; }
        void RunPhysics(Cells.Cell cell);
        bool KillNextFrame { get; }

        bool DrawDebugInfo { get; set; }

        [Obsolete("Only set by deprecated methods")]
        Position[]? LastProjectedPath { get; set; }
    }
}
