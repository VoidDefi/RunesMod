using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria.GameContent.Creative;
using RunesMod.ModUtils;
using RunesMod.Items.Materials.Bars;
using Terraria.GameContent;

namespace RunesMod.Items.Placeables.Ores
{
    public class SulfurItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;
            Item.ResearchUnlockCount = 100;
            ItemID.Sets.SortingPriorityMaterials[Type] = 80;

            ItemTrader.ChlorophyteExtractinator.AddOption_OneWay(Type, 1, ModContent.ItemType<CinnabarItem>(), 1);
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;

            Item.createTile = ModContent.TileType<Tiles.Ores.Sulfur>(); 
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.consumable = true;

            Item.maxStack = 9999;
           
            Item.value = Item.sellPrice(silver: 4);
            Item.rare = ItemRarityID.Green;
        }
    }
}
