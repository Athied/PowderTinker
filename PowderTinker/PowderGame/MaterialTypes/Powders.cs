using Raylib_cs;

namespace PowderGame
{
    public class Sand : BaseMaterial
    {
        public override MaterialTypes MaterialType { get { return MaterialTypes.Powder; } }

        public override string Name { get { return "Sand"; } }
        public override ColorRange Colors { get { return new ColorRange(new Color(239, 207, 160, 255), new Color(231, 182, 109, 255)); } }

        public override float OverallSpeed { get { return 3; } }
    }
}