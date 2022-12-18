using Raylib_cs;

namespace PowderGame.Materials
{
    public class Sand : Powder
    {
        public override string Name { get { return "Sand"; } }
        public override ColorRange Colors { get { return new ColorRange(new Color(239, 207, 160, 255), new Color(231, 182, 109, 255)); } }

        public override float PourSpeed { get { return 10; } }
        public override float Density { get { return 700; } }
        public override float DragResistance { get { return 25; } }

        public override float OverallSpeed { get { return 3; } }
    }
}
