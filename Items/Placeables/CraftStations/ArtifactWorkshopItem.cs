using Newtonsoft.Json.Linq;
using RunesMod.Tiles.CraftStations;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RunesMod.Items.Placeables.CraftStations
{
    public class ArtifactWorkshopItem : ModItem
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
            Item.createTile = ModContent.TileType<ArtifactWorkshop>();
            Item.rare = ItemRarityID.Blue;
        }

        public override void AddRecipes()
        {
            //Recipe recipe = CreateRecipe();
            //recipe.Register();
        }
    }
}
