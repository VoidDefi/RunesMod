using Newtonsoft.Json.Linq;
using RunesMod.Tiles.CraftStations;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RunesMod.Items.Placeables.CraftStations
{
    public class SimpleAlchemicalTableItem : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = Terraria.Item.CommonMaxStack;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = 1;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<SimpleAlchemicalTable>();
            Item.rare = ItemRarityID.Blue;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddRecipeGroup(RecipeGroupID.Wood, 8);
            recipe.AddIngredient(ItemID.Torch, 1);
            recipe.AddIngredient(ItemID.Bottle, 1);
            recipe.AddIngredient(ItemID.Book, 2);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
