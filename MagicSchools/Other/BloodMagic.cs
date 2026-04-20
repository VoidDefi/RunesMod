using RunesMod.MagicSchools.Elemental;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunesMod.MagicSchools.Other
{
    public class BloodMagic : MagicSchool
    {
        public override bool NeedVoidMana => false; 

        public override void SetStaticDefaults()
        {

        }

        public override bool CanStack(MagicSchool school)
        {
            return false;
        }
    }
}