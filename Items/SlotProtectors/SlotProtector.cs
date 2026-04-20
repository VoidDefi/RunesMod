using Microsoft.Xna.Framework;
using RunesMod.MagicSchools.Other;
using RunesMod.MagicSchools;
using RunesMod.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace RunesMod.Items.SlotProtectors
{
    public abstract class SlotProtector : ModItem
    {
        public override void SetDefaults()
        {

        }

        public override bool CanUseItem(Player player)
        {
            return false;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            base.ModifyTooltips(tooltips);
        }

        public virtual bool CanUseSpell(Player player, Spell spell)
        {
            return spell.magicSchools?.Contains(MagicSchoolLoader.SchoolType<BloodMagic>()) != true;
        }
    }
}
