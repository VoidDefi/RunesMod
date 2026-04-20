using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.Localization;
using Terraria;
using Terraria.ModLoader;
using RunesMod.Items.Materials.Bars;

namespace RunesMod.Items.Armor.CinnabarSet
{
    [AutoloadEquip(EquipType.Legs)]
    public class CinnabarLeggings : ModItem
    {
        public static readonly int MoveSpeed = 5;

        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(MoveSpeed);

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(gold: 1);
            Item.rare = ItemRarityID.Orange;
            Item.defense = 4;
        }

        public override void UpdateEquip(Player player)
        {
            player.moveSpeed += MoveSpeed / 100f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient<CinnabarBar>(7);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
