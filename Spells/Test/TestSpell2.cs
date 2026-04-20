using Microsoft.Xna.Framework;
using RunesMod.ModUtils;
using RunesMod.Projectiles.Magic.Elements.Fire;
using RunesMod.Shields;
using RunesMod.Shields.Elements.Fire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace RunesMod.Spells.Test
{
    public class TestSpell2 : Spell
    {
        protected override void SetDefaults()
        {
            costType = CostTypes.Mana;
            cost = 10;

            cooldownTime = 20;
            animationTime = 20;
            
            flipPlayer = false;

            sustainable = true;
            occupiedSlotLength = 1;
            concentrationBarGradient = new(Color.Blue, 0.3f);
        }

        public override void OnCasting(Player player, Vector2 velocity)
        {

        }
    }
}
