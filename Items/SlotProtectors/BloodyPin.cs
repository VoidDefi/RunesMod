using RunesMod.Items.Materials;
using RunesMod.Items.Placeables.Ores;
using RunesMod.MagicSchools;
using RunesMod.MagicSchools.Other;
using RunesMod.Spells;
using RunesMod.Tiles.Ores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace RunesMod.Items.SlotProtectors
{
    public class BloodyPin : SlotProtector
    {
        public override void SetDefaults()
        {
            int delay = (int)(1f * 100);

            Item.useTime = delay;
            Item.useAnimation = delay;
            Item.width = 14;
            Item.height = 30;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(1);
            recipe.AddRecipeGroup(RecipeGroupID.IronBar, 2);
            recipe.AddIngredient<CinnabarItem>(5);
            recipe.Register();
        }

        public override bool CanUseSpell(Player player, Spell spell)
        {
            return true;
        }
    }
}
