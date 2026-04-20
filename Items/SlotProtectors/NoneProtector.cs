
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace RunesMod.Items.SlotProtectors
{
    public class NoneProtector : SlotProtector
    {
        public override void SetDefaults()
        {
            int delay = (int)(1f * 100);

            Item.useTime = delay;
            Item.useAnimation = delay;
            Item.width = 8;
            Item.height = 8;
        }
    }
}
