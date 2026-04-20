using Microsoft.Xna.Framework;
using RunesMod.CastingAnimations;
using RunesMod.MagicSchools.Elemental;
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

namespace RunesMod.Spells.Defense.Elements.Fire
{
    public class FireShieldSpell : Spell
    {
        protected override void SetDefaults()
        {
            costType = CostTypes.Mana;
            cost = 10;

            cooldownTime = 20;
            animationTime = 20;
            animationStyle = CastingAnimationLoader.AnimationType<RaisingHandsAnimation>();

            flipPlayer = false;

            sustainable = true;
            occupiedSlotLength = 1;

            concentrationBarGradient = new
            (
                new(255, 237, 166), 
                new(255, 153, 0), 
                new(182, 55, 0), 
                new(134, 27, 0)
            );

            AddMagicSchool<FireMagic>();
        }

        public override void OnCasting(Player player, Vector2 velocity)
        {
            if (player.whoAmI != Main.myPlayer) return;

            player.ShieldSystem().AddShield(ShieldLoader.ShieldType<FireShield>(), this);
        }
    }
}
