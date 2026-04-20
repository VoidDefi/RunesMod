using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RunesMod.Items.Placeables;
using RunesMod.Systems.Crafting;
using RunesMod.Spells;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace RunesMod.Items.Materials
{
    public class PrismOfIcerianCrystal : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
            Item.ResearchUnlockCount = 25;
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 34;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(silver: 8);
            Item.rare = ItemRarityID.Green;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<RockCrystalItem>(), 20);
            recipe.AddIngredient(ItemID.IceBlock, 20);
            recipe.AddIngredient(ModContent.ItemType<VoidFragment>(), 1);
            recipe.AddStatConsume(CostTypes.Mana, 20);
            recipe.AddTile(TileID.Furnaces);
            recipe.Register();
        }
    }
}