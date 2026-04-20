using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RunesMod.Dusts
{
    public class GlowingPowderDust : ModDust
    {
        public override void SetStaticDefaults()
        {
            UpdateType = DustID.PurificationPowder;
        }
    }
}
