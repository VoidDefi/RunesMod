using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using RunesMod.Tiles.CraftStations;
using RunesMod.Items.ArtifactItems.Blanks;

namespace RunesMod.Systems
{
    public class ModifyRecipes : ModSystem
    {
        public override void PostAddRecipes()
        {
            for (int i = 0; i < Recipe.numRecipes; i++)
            {
                Recipe recipe = Main.recipe[i];

                TryRemoveRecipe(recipe, 4131); //Void Bag
                TryRemoveRecipe(recipe, ItemID.VoidVault);
                TryRemoveRecipe(recipe, ItemID.MagicMirror);

                if (HasResults(recipe, ItemID.VilePowder, ItemID.ViciousPowder)) 
                {
                    recipe.RemoveTile(TileID.Bottles);
                    recipe.AddTile<Mortar>();
                }
            }

            Recipe newRecipe = null;

            newRecipe = Recipe.Create(ItemID.MagicMirror, 1);
            newRecipe.AddIngredient<MirrorBlank>(1);
            newRecipe.AddTile<ArtifactWorkshop>();
            newRecipe.Register();

            newRecipe = Recipe.Create(ItemID.IceMirror, 1);
            newRecipe.AddIngredient(ItemID.MagicMirror, 1);
            newRecipe.AddIngredient(ItemID.IceBlock, 10);
            newRecipe.AddTile(TileID.Anvils);
            newRecipe.Register();
        }

        private void TryRemoveRecipe(Recipe recipe, int resultType)
        {
            if (recipe.HasResult(resultType))
            {
                recipe.DisableRecipe();
            }
        }

        private bool HasResults(Recipe recipe, params int[] items)
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (recipe.HasResult(items[i]))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
