using RunesMod.Items.Materials;
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
    public class PieceOfWood : SlotProtector
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

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
            recipe.AddRecipeGroup(RecipeGroupID.Wood, 10);
            recipe.Register();
        }
    }
}
