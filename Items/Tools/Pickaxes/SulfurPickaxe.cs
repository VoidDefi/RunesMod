using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.UI;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using RunesMod.Items.Materials.Bars;

namespace RunesMod.Items.Tools.Pickaxes
{
    public class SulfurPickaxe : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 40;
            Item.damage = 8;
            Item.DamageType = DamageClass.Melee;
            Item.knockBack = 3f;

            Item.useTime = 16;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            
            Item.value = Item.buyPrice(silver: 60);
            Item.rare = ItemRarityID.Orange;
            
            Item.pick = 105;
            Item.attackSpeedOnlyAffectsWeaponAnimation = true;
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {

        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient<SulfurBar>(10);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
