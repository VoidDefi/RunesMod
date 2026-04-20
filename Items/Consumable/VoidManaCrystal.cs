using RunesMod.Items.Materials;
using RunesMod.Rarities;
using RunesMod.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace RunesMod.Items.Consumable
{
    public class VoidManaCrystal : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 1;
            Item.consumable = true;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.UseSound = SoundID.Item4;
            Item.rare = ModContent.RarityType<VoidRarity>();
        }

        public override bool CanUseItem(Player player)
        {
            return !player.GetModPlayer<PlayerStates>().TrueMagic && player.statLife > 1;
        }

        public override bool? UseItem(Player player)
        {
            if (player.statLife > 1)
            {
                Player.HurtInfo hurt = new();
                hurt.Damage = player.statLife - 1;
                hurt.Dodgeable = false;
                hurt.DamageSource = new();

                player.Hurt(hurt);

                player.SetImmuneTimeForAllTypes(120);
                
                player.GetModPlayer<PlayerStates>().TrueMagic = true;
            }

            return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.FallenStar, 10);
            recipe.AddIngredient<VoidFragment>(5);
            recipe.Register();
        }
    }
}
