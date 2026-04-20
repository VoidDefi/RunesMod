using RunesMod.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace RunesMod.Qualities.Potions
{
    public class Perfect : PotionQuality
    {
        public Perfect(Item item) : base(item)
        {
            type = 4;
            timeFactor = 1.4f;
            statFactor = 1f;
        }
    }
}
