using Microsoft.Xna.Framework;
using RunesMod.Projectiles.Magic.Elements.Fire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using RunesMod.Projectiles.Magic.Elements.Ice;
using RunesMod.CastingAnimations;
using RunesMod.MagicSchools.Elemental;

namespace RunesMod.Spells.Elements.Ice
{
    public  class FrostyBreathSpell : Spell
    {
        protected override void SetDefaults()
        {
            costType = CostTypes.Mana;
            cost = 15;

            cooldownTime = 30;
            animationTime = 30;
            animationStyle = CastingAnimationLoader.AnimationType<SwingAnimation>();

            projectileType = ModContent.ProjectileType<FrostyMist>();
            projectileSpeed = 10f;
            damage = 20;
            knockBack = 0.05f;

            AddMagicSchool<IceMagic>();
            AddMagicSchool<AirMagic>();
        }
    }
}
