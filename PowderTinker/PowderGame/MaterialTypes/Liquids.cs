using Raylib_cs;

namespace PowderGame
{
    public class Water : BaseMaterial
    {
        public override MaterialTypes MaterialType { get { return MaterialTypes.Liquid; } }

        public override string Name { get { return "Water"; } }
        public override ColorRange Colors { get { return new ColorRange(new Color(20, 122, 248, 255), new Color(20, 163, 248, 255)); } }

        public override float OverallSpeed { get { return 4; } }
    }
}