using RunesMod.Items.Materials;
using RunesMod.Items.Placeables;
using RunesMod.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using RunesMod.Tiles.CraftStations;

namespace RunesMod.Items.ArtifactItems.Blanks
{
    public class MirrorBlank : ModItem
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
            Item.value = Item.sellPrice(silver: 20);
            Item.rare = ItemRarityID.Green;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.SilverBar, 5);
            recipe.AddIngredient<ManaAccumulatorCrystal>(6);
            recipe.AddIngredient<PrismOfIcerianCrystal>(1);
            recipe.AddTile<ArtifactWorkshop>();
            recipe.Register();

            recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.TungstenBar, 5);
            recipe.AddIngredient<ManaAccumulatorCrystal>(6);
            recipe.AddIngredient<PrismOfIcerianCrystal>(1);
            recipe.AddTile<ArtifactWorkshop>();
            recipe.Register();
        }
    }
}
