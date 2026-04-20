using Newtonsoft.Json.Linq;
using RunesMod.Tiles.CraftStations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RunesMod.Items.Placeables.CraftStations
{
    public class DistillerItem : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = Item.CommonMaxStack;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = 1;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<Distiller>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.CookingPot);
            recipe.AddIngredient(ItemID.Glass, 5);
            recipe.AddTile(TileID.GlassKiln);
            recipe.Register();
        }
    }
}
