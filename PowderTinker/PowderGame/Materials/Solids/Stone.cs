using Raylib_cs;

namespace PowderGame.Materials
{
    public class Stone : NonStaticSolid
    {
        public override string Name { get { return "name"; } }
        public override ColorRange Colors { get { return new ColorRange(Color.GRAY, Color.DARKGRAY); } }

        public override float OverallSpeed { get { return 4; } }
    }
}
