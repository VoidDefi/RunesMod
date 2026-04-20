using RunesMod.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace RunesMod.Qualities.Potions
{
    public class Defective : PotionQuality
    {
        public Defective(Item item) : base(item)
        {
            type = 1;
            timeFactor = 0.6f;
            statFactor = 1f;
        }
    }
}
