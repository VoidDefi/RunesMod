using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria.GameContent.Creative;
using RunesMod.Tiles.Ores;

namespace RunesMod.Items.Placeables.Ores
{
    public class RockCrystalOreItem : ModItem
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

            Item.createTile = ModContent.TileType<RockCrystalOre>();
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.consumable = true;

            Item.maxStack = 9999;

            Item.value = Item.sellPrice(silver: 2, copper: 80);
            Item.rare = ItemRarityID.Blue;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<RockCrystalItem>(), 2);
            recipe.AddTile(TileID.Furnaces);
            recipe.Register();
        }
    }
}
