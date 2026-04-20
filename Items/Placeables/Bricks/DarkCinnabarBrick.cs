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
using RunesMod.Items.Placeables.Ores;
using RunesMod.Tiles.Bricks;

namespace RunesMod.Items.Placeables.Bricks
{
    public class DarkCinnabarBrick : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;
            Item.ResearchUnlockCount = 100;
            ItemID.Sets.SortingPriorityMaterials[Type] = 80;
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;

            Item.createTile = ModContent.TileType<DarkCinnabarBrickTile>(); 
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.consumable = true;

            Item.maxStack = 9999;
           
            Item.rare = ItemRarityID.White;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(5);
            recipe.AddIngredient(ItemID.StoneBlock, 5);
            recipe.AddIngredient<CinnabarItem>(2);
            recipe.AddTile(TileID.Furnaces);
            recipe.Register();
        }
    }
}
