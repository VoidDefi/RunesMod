using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using System.Runtime.CompilerServices;
using Terraria.Localization;
using Newtonsoft.Json.Linq;

namespace RunesMod.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
    public class UniversalRobe : ModItem
    {
        private static int Legs { get; set; } = -1;

        public static int MaxMana => 80;

        public static int ManaCost => 20;

        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(MaxMana, ManaCost);

        public override void Load()
        {
            if (!Main.dedServ)
            {
                string path = Texture + "_Legs";
                Legs = EquipLoader.AddEquipTexture(Mod, path, EquipType.Legs, this);
            }
        }

        public override void SetMatch(bool male, ref int equipSlot, ref bool robes)
        {
            robes = true;
            equipSlot = Legs;
        }

        public override void SetStaticDefaults()
        {
            ArmorIDs.Body.Sets.HidesHands[Item.bodySlot] = false;
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 14;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(gold: 2, silver: 40);
            Item.defense = 4;
        }

        public override void UpdateEquip(Player player)
        {
            player.hasGemRobe = true;
            player.statManaMax2 += MaxMana;
            player.manaCost -= ManaCost / 100f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.DiamondRobe);
            recipe.AddIngredient(ItemID.Amethyst, 5);
            recipe.AddIngredient(ItemID.Topaz, 5);
            recipe.AddIngredient(ItemID.Sapphire, 5);
            recipe.AddIngredient(ItemID.Emerald, 5);
            recipe.AddIngredient(ItemID.Ruby, 5);
            recipe.AddIngredient(ItemID.Amber, 5);
            recipe.AddTile(TileID.Loom);
            recipe.Register();
        }
    }

}
