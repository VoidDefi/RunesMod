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
    [AutoloadEquip(EquipType.Head)]
    public class CinnabarCrown : ModItem
    {
        public static LocalizedText SetBonusText { get; private set; }

        public static int MaxMana => 20;

        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(MaxMana);

        public override void SetStaticDefaults()
        {
            if (Main.netMode != NetmodeID.Server)
                ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true;

            SetBonusText = this.GetLocalization("SetBonus").WithFormatArgs
            (
                Language.GetTextValue("BuffName.OnFire"),
                Language.GetTextValue("BuffName.Burning"),
                0, 0
            );
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(gold: 1);
            Item.rare = ItemRarityID.Orange;
            Item.defense = 4;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<CinnabarBreastplate>() &&
                   legs.type == ModContent.ItemType<CinnabarLeggings>();
        }

        public override void UpdateEquip(Player player)
        {
            player.statManaMax2 += MaxMana;
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = SetBonusText.Value;

            player.buffImmune[BuffID.OnFire] = true;
            player.buffImmune[BuffID.Burning] = true;

            //player.GetDamage(DamageClass.Generic) += AdditiveGenericDamageBonus / 100f; // Increase dealt damage for all weapon classes by 20%
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient<CinnabarBar>(5);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
