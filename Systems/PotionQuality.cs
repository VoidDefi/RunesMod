using RunesMod.Buffs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace RunesMod.Systems
{
    public abstract class PotionQuality
    {
        public int type = 0;

        public float timeFactor = 1.0f;
        public float statFactor = 1.0f;

        public Item Item { get; set; }

        public virtual LocalizedText Name => Language.GetText($"Mods.RunesMod.PotionQualities.{GetType().Name}");

        public virtual LocalizedText Description => Language.GetText($"Mods.RunesMod.PotionQualities.{GetType().Name}Description");

        public PotionQuality(Item item)
        {
            Item = item;
        }

        public virtual void OnUseItem(Player player, Item item)
        {

        }

        protected virtual void AddExtraDeBuffs(Player player, Item item, int time) 
        {
            if (item.type == ItemID.IronskinPotion || item.type == ItemID.ObsidianSkinPotion)
            {
                player.AddBuff(ModContent.BuffType<StoneEntrails>(), time);
            }

            if (item.type == ItemID.SpelunkerPotion || item.type == ItemID.NightOwlPotion || item.type == ItemID.HunterPotion)
            {
                player.AddBuff(BuffID.Darkness, time);
            }
        }
    }
}
