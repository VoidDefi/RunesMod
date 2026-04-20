using RunesMod.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace RunesMod.Qualities.Potions
{
    public class Clear : PotionQuality
    {
        public Clear(Item item) : base(item)
        {
            type = 3;
            timeFactor = 1.2f;
            statFactor = 1f;
        }
    }
}
