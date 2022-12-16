using Raylib_cs;

namespace PowderGame
{
    public class Stone : BaseMaterial
    {
        public override MaterialTypes MaterialType { get { return MaterialTypes.Solid; } }

        public override string Name { get { return "Stone"; } }
        public override ColorRange Colors { get { return new ColorRange(Color.GRAY, Color.DARKGRAY); } }

        public override float OverallSpeed { get { return 4; } }
    }
}