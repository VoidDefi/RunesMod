using RunesMod.Items;
using RunesMod.Items.Consumable;
using RunesMod.Items.Placeables.CraftStations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RunesMod.Systems
{
    public class OtherPlayer : ModPlayer
    {
        public override IEnumerable<Item> AddStartingItems(bool mediumCoreDeath)
        {
            List<Item> items = new List<Item>();
            items.Add(new Item(ModContent.ItemType<VoidManaCrystal>()));
            items.Add(new Item(ItemID.HealingPotion));
            items.Add(new Item(ModContent.ItemType<ScrollFire>()));
            items.Add(new Item(ModContent.ItemType<ArtifactWorkshopItem>()));

            return items;
        }
    }
}
