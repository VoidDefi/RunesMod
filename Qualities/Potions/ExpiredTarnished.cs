using RunesMod.Buffs;
using RunesMod.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RunesMod.Qualities.Potions
{
    public class ExpiredTarnished : PotionQuality
    {
        public ExpiredTarnished(Item item) : base(item)
        {
            type = 8;
            timeFactor = 0.5f;
            statFactor = 1f;
        }

        public override void OnUseItem(Player player, Item item)
        {
            int time = (int)((float)item.buffTime * 0.75f);

            int count = Main.rand.Next(3);
            int rand = Main.rand.Next(5);

            for (int i = 0; i < count; i++)
            {
                if (rand == 0) player.AddBuff(BuffID.Poisoned, time);
                else if(rand == 1) player.AddBuff(ModContent.BuffType<HighTemperature>(), time);
                else if (rand >= 2 || rand <= 3) player.AddBuff(ModContent.BuffType<Nausea>(), time);
                else
                {
                    AddExtraDeBuffs(player, item, time);
                }
            }
        }
    }
}
