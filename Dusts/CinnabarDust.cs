using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;

namespace RunesMod.Dusts
{
    public class CinnabarDust : ModDust
    {
        public override void SetStaticDefaults()
        {
            UpdateType = DustID.Stone;
        }
    }
}
