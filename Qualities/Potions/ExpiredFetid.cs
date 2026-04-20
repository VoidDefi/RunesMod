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
    public class ExpiredFetid : PotionQuality
    {
        public ExpiredFetid(Item item) : base(item)
        {
            type = 5;
            timeFactor = 0.35f;
            statFactor = 1f;
        }

        public override void OnUseItem(Player player, Item item)
        {
            int time = (int)((float)item.buffTime * 2f);

            int count = Main.rand.Next(1, 4);
            int rand = Main.rand.Next(11);

            for (int i = 0; i < count; i++)
            {
                if (rand == 0) player.AddBuff(BuffID.Poisoned, time);
                else if (rand == 1) player.AddBuff(ModContent.BuffType<HighTemperature>(), time);
                else if (rand >= 2 || rand <= 4) player.AddBuff(ModContent.BuffType<Nausea>(), time);
                else if (rand == 5 || rand <= 7) player.AddBuff(ModContent.BuffType<Dizziness>(), time);
                else if (rand == 8) player.AddBuff(BuffID.Weak, time);
                else if (rand == 9) player.AddBuff(BuffID.Confused, time);
                else
                {
                    AddExtraDeBuffs(player, item, time);
                }
            }
        }
    }
}
