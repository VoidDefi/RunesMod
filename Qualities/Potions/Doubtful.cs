using RunesMod.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace RunesMod.Qualities.Potions
{
    public class Doubtful : PotionQuality
    {
        public Doubtful(Item item) : base(item)
        {
            type = 2;
            timeFactor = 0.8f;
            statFactor = 1f;
        }
    }
}
