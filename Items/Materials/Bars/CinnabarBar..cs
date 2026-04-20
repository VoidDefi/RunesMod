using RunesMod.Items.Placeables;
using RunesMod.Items.Placeables.Ores;
using RunesMod.Tiles.Bars;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace RunesMod.Items.Materials.Bars
{
    public class CinnabarBar : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 25;
            ItemID.Sets.SortingPriorityMaterials[Type] = 71;

            ItemTrader.ChlorophyteExtractinator.AddOption_OneWay(Type, 1, ModContent.ItemType<SulfurBar>(), 1);
        }

        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<CinnabarBarTile>());
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(silver: 50);
            Item.rare = ItemRarityID.Green;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            
            recipe.AddIngredient(ItemID.HellstoneBar);
            recipe.AddIngredient<CinnabarItem>(3);
            recipe.AddTile(TileID.Hellforge);
            recipe.Register();
        }
    }
}
