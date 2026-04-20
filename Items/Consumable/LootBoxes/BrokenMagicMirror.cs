using RunesMod.Items.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace RunesMod.Items.Consumable.LootBoxes
{
    public class BrokenMagicMirror : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
        }

        public override void SetDefaults()
        {
            Item.maxStack = 9999;
            Item.width = 12;
            Item.height = 12;
            Item.rare = ItemRarityID.Gray;
            Item.value = Item.sellPrice(0, 0, 10);
        }

        public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup)
        {
            itemGroup = ContentSamples.CreativeHelper.ItemGroup.Crates;
        }

        public override bool CanRightClick()
        {
            return true;
        }

        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            IItemDropRule[] bars =
            {
                ItemDropRule.Common(ItemID.SilverBar, 1, 1, 5),
                ItemDropRule.Common(ItemID.TungstenBar, 1, 1, 5),
            };
            itemLoot.Add(new OneFromRulesRule(1, bars));

            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<ManaAccumulatorCrystal>(), 1, 1, 3));
        }
    }
}
