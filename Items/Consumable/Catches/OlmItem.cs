using RunesMod.NPCs;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace RunesMod.Items.Consumable.Catches
{
    public class OlmItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.Goldfish);
            Item.makeNPC = ModContent.NPCType<Olm>();
            Item.value += Item.buyPrice(0, 0, 10, 0);
            Item.rare = ItemRarityID.White;
        }
    }
}
