using Microsoft.Xna.Framework;
using RunesMod.CastingAnimations;
using RunesMod.MagicSchools.Elemental;
using RunesMod.Projectiles.Magic.Elements.Earth;
using RunesMod.Projectiles.Magic.Elements.Electric;
using RunesMod.Projectiles.Magic.Elements.Fire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace RunesMod.Spells.Elements.Electric
{
    public class LightingSpell : Spell
    {
        protected override void SetDefaults()
        {
            costType = CostTypes.Mana;
            cost = 10;

            cooldownTime = 16;
            animationTime = 16;
            animationStyle = CastingAnimationLoader.AnimationType<PushAnimation>();

            projectileType = ModContent.ProjectileType<LightingProjectile>();
            projectileSpeed = 10f;
            damage = 29;
            knockBack = 2.5f;

            AddMagicSchool<ElectricMagic>();
        }

        public override void OnCasting(Player player, Vector2 velocity)
        {
            base.OnCasting(player, velocity);
        }
    }
}
