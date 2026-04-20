using Microsoft.Xna.Framework;
using RunesMod.CastingAnimations;
using RunesMod.MagicSchools.Elemental;
using RunesMod.Projectiles.Magic.Elements.Earth;
using RunesMod.Projectiles.Magic.Elements.Fire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace RunesMod.Spells.Elements.Earth
{
    public class StalactiteSpell : Spell
    {
        protected override void SetDefaults()
        {
            costType = CostTypes.Mana;
            cost = 10;

            cooldownTime = 14;
            animationTime = 14;
            animationStyle = CastingAnimationLoader.AnimationType<PushAnimation>();

            projectileType = ModContent.ProjectileType<Stalactite>();
            projectileSpeed = 21f;
            damage = 15;
            knockBack = 8.5f;

            AddMagicSchool<EarthMagic>();
        }

        public override void OnCasting(Player player, Vector2 velocity)
        {
            base.OnCasting(player, velocity);
        }
    }
}
