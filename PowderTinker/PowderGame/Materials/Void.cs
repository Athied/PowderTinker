using Raylib_cs;

namespace PowderGame.Materials
{
    public class Void : BaseMaterial
    {
        public override MaterialTypes MaterialType { get { return MaterialTypes.None; } }

        public override string Name { get { return "None"; } }
        public override ColorRange Colors { get { return new ColorRange(Color.BLACK, Color.BLACK); } }

        public override float OverallSpeed { get { return 0; } }
    }
}
