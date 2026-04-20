using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.Localization;
using Terraria;
using Terraria.ModLoader;
using RunesMod.Items.Materials;
using RunesMod.Items.Placeables;
using RunesMod.Spells;
using RunesMod.Systems.Crafting;
using RunesMod.Items.Materials.Bars;

namespace RunesMod.Items.Armor.CinnabarSet
{
    [AutoloadEquip(EquipType.Body)]
    public class CinnabarBreastplate : ModItem
    {
        public static int MaxMana => 40;

        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(MaxMana);

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(gold: 1);
            Item.rare = ItemRarityID.Orange;
            Item.defense = 5;
        }

        public override void UpdateEquip(Player player)
        {
            player.statManaMax2 += MaxMana;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient<CinnabarBar>(10);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
