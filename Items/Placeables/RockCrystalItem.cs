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

namespace RunesMod.Items.Placeables
{
    public class RockCrystalItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;
            Item.ResearchUnlockCount = 100;
            ItemID.Sets.SortingPriorityMaterials[Type] = 80;
        }

        public override void SetDefaults()
        {
            Item.width = 8;
            Item.height = 8;

            Item.createTile = ModContent.TileType<Tiles.RockCrystal>(); 
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.consumable = true;

            Item.maxStack = 9999;
           
            Item.value = Item.sellPrice(copper: 70);
            Item.rare = ItemRarityID.Blue;
        }
    }
}
