using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using System.Reflection;
using RunesMod.Systems;
using Terraria.GameContent;
using ReLogic.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.WorldBuilding;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;

namespace RunesMod
{
	public class RunesMod : Mod
	{
        public static bool[] VanillaPotions { get; private set; } = new bool[0];
 
        public static bool IsPotion(int type)
        {
            if (VanillaPotions.Length > type)
            {
                if (VanillaPotions[type])
                {
                    return true;
                }
            }

            return false;
        }

        public override void PostSetupContent()
        {
            PotionQualitySystem.InitQualities();

            if (!Main.dedServ)
            {
                Asset<Effect> TrailShader = ModAssets.Request<Effect>(ModAssets.Effects, "Trail");
                GameShaders.Misc["RunesMod:Trail"] = new MiscShaderData(TrailShader, "ShaderPass");

                Asset<Effect> StaticTrailShader = ModAssets.Request<Effect>(ModAssets.Effects, "StaticTrail");
                GameShaders.Misc["RunesMod:StaticTrail"] = new MiscShaderData(StaticTrailShader, "ShaderPass");
            }
        }

        public override void PostAddRecipes()
        {
            #region Set vanilla potions

            VanillaPotions = new bool[ItemLoader.ItemCount];

            for (int i = 288; i <= 305; i++) //288-305
                VanillaPotions[i] = true;

            for (int i = 2322; i <= 2329; i++) //2322-2329
                VanillaPotions[i] = true;

            for (int i = 2344; i <= 2356; i++) //2344-2356
                VanillaPotions[i] = true;

            for (int i = 4477; i <= 4479; i++) //4477-4479
                VanillaPotions[i] = true;

            VanillaPotions[2359] = true;
            VanillaPotions[5211] = true;
            VanillaPotions[2756] = true;
            VanillaPotions[4870] = true;

            VanillaPotions[28] = true;
            VanillaPotions[110] = true;
            VanillaPotions[188] = true;
            VanillaPotions[189] = true;
            VanillaPotions[226] = true;
            VanillaPotions[227] = true;
            VanillaPotions[499] = true;
            VanillaPotions[500] = true;
            VanillaPotions[678] = true;
            VanillaPotions[2209] = true;
            VanillaPotions[2997] = true;
            VanillaPotions[3544] = true;

            #endregion

            List<Recipe> newRecipes = new();

            for (int i = 0; i < Recipe.numRecipes; i++)
            {
                Recipe recipe = Main.recipe[i];

                if (recipe.createItem != null)
                {
                    if (VanillaPotions[recipe.createItem.type])
                        Main.recipe[i] = null;
                }

                if (Main.recipe[i] != null)
                    newRecipes.Add(recipe);
            }

            
            Recipe.numRecipes = newRecipes.Count;

            PropertyInfo recipeIndexInfo = typeof(Recipe).GetProperty("RecipeIndex");
            ConstructorInfo recipeConstructor = typeof(Recipe).GetConstructor
            (
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public,
                null,
                new Type[] { typeof(Mod) },
                null
            );

            for (int i = 0; i < Recipe.maxRecipes; i++)
            {
                if (i < newRecipes.Count)
                    Main.recipe[i] = newRecipes[i];

                else
                    Main.recipe[i] = (Recipe)recipeConstructor.Invoke(new object[] { null });

                recipeIndexInfo.SetValue(Main.recipe[i], i);
            }
        }
    }
}
