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
    public class ExpiredStale : PotionQuality
    {
        public ExpiredStale(Item item) : base(item)
        {
            type = 6;
            timeFactor = 0.4f;
            statFactor = 1f;
        }

        public override void OnUseItem(Player player, Item item)
        {
            int time = (int)((float)item.buffTime * 1.5f);

            int count = Main.rand.Next(2, 6);
            int rand = Main.rand.Next(10);

            for (int i = 0; i < count; i++)
            {
                if (rand == 0) player.AddBuff(BuffID.Poisoned, time);
                else if (rand == 1) player.AddBuff(ModContent.BuffType<HighTemperature>(), time);
                else if (rand >= 2 || rand <= 4) player.AddBuff(ModContent.BuffType<Nausea>(), time);
                else if (rand == 5 || rand <= 7) player.AddBuff(ModContent.BuffType<Dizziness>(), time);
                else if (rand == 8) player.AddBuff(BuffID.Weak, time);
                else
                {
                    AddExtraDeBuffs(player, item, time);
                }
            }
        }
    }
}
