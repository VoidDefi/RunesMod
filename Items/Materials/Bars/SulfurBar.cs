using RunesMod.Items.Placeables;
using RunesMod.Items.Placeables.Ores;
using RunesMod.Spells;
using RunesMod.Tiles.Bars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace RunesMod.Items.Materials.Bars
{
    public class SulfurBar : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 25;
            ItemID.Sets.SortingPriorityMaterials[Type] = 71;

            ItemTrader.ChlorophyteExtractinator.AddOption_OneWay(Type, 1, ModContent.ItemType<CinnabarBar>(), 1);
        }

        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<SulfurBarTile>());
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(silver: 50);
            Item.rare = ItemRarityID.Green;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            
            recipe.AddIngredient(ItemID.HellstoneBar);
            recipe.AddIngredient<SulfurItem>(3);
            recipe.AddTile(TileID.Hellforge);
            recipe.Register();
        }
    }
}
