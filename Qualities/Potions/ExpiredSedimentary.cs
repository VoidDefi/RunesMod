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
    public class ExpiredSedimentary : PotionQuality
    {
        public ExpiredSedimentary(Item item) : base(item)
        {
            type = 7;
            timeFactor = 0.45f;
            statFactor = 1f;
        }

        public override void OnUseItem(Player player, Item item)
        {
            int time = (int)((float)item.buffTime * 1f);

            int count = Main.rand.Next(4, 8);
            int rand = Main.rand.Next(7);

            for (int i = 0; i < count; i++)
            {
                if (rand == 0) player.AddBuff(BuffID.Poisoned, 600);
                else if (rand == 1) player.AddBuff(ModContent.BuffType<HighTemperature>(), time);
                else if (rand >= 2 || rand <= 3) player.AddBuff(ModContent.BuffType<Nausea>(), time);
                else if (rand == 4) player.AddBuff(ModContent.BuffType<Dizziness>(), time);
                else if (rand == 5) player.AddBuff(BuffID.Weak, time);
                else
                {
                    AddExtraDeBuffs(player, item, time);
                }
            }
        }
    }
}
