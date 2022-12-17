using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowderGame.Materials
{
    public abstract class StaticSolid : BaseMaterial
    {
        public sealed override MaterialTypes MaterialType { get { return MaterialTypes.Solid; } }

        public override void Physics()
        {
            base.Physics();
        }
    }
}
