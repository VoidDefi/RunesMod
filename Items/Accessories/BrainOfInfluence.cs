using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;
using RunesMod.ModUtils;

namespace RunesMod.Items.Accessories
{
    [AutoloadEquip(EquipType.Face)]
    public class BrainOfInfluence : ModItem
    {
        private Item brainOfConfusion = null;

        public override LocalizedText Tooltip => base.Tooltip;

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;

            Item.accessory = true;
            Item.rare = ItemRarityID.Expert;
            Item.value = Item.buyPrice(gold: 3);
            Item.expert = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (brainOfConfusion == null || Item.prefix != (brainOfConfusion?.prefix ?? Item.prefix))
                brainOfConfusion = new(ItemID.BrainOfConfusion, 1, Item.prefix);

            player.brainOfConfusionItem = brainOfConfusion;
            player.SpellSystem().concentration.MaxConcentration += 1;

            if (!hideVisual)
            {

            }
        }

        public override void UpdateVanity(Player player)
        {

        }
    }
}
