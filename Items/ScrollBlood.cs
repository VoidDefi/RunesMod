using RunesMod.ModUtils;
using RunesMod.Spells;
using RunesMod.Spells.Blood;
using RunesMod.Spells.Elements.Electric;
using RunesMod.Spells.Elements.Fire;
using RunesMod.Spells.Elements.Ice;
using RunesMod.Spells.Elements.Water;
using RunesMod.Systems.Knowledges;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RunesMod.Items
{
    public class ScrollBlood : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 8;
            Item.height = 8;
            Item.maxStack = 9999;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.UseSound = SoundID.Item4;
        }

        public override bool? UseItem(Player player)
        {
            SpellKnowledge spell = new(SpellLoader.GetSpell<BloodBoltSpell>());

            player.KnowledgeSystem().Learning(spell);

            SpellKnowledge spell1 = new(SpellLoader.GetSpell<BloodScytheSpell>());

            player.KnowledgeSystem().Learning(spell1);

            return true;
        }
    }
}
