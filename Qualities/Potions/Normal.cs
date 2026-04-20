using RunesMod.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace RunesMod.Qualities.Potions
{
    public class Normal : PotionQuality
    {
        public Normal(Item item) : base(item)
        {
            type = 0;
            timeFactor = 1f;
            statFactor = 1f;
        }
    }
}
