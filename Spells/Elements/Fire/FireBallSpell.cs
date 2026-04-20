using Microsoft.Xna.Framework;
using RunesMod.CastingAnimations;
using RunesMod.MagicSchools.Elemental;
using RunesMod.Projectiles.Magic.Elements.Fire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace RunesMod.Spells.Elements.Fire
{
    public class FireBallSpell : Spell
    {
        protected override void SetDefaults()
        {
            costType = CostTypes.Mana;
            cost = 10;

            cooldownTime = 14;
            animationTime = 14;
            animationStyle = CastingAnimationLoader.AnimationType<RotateAnimation>();

            projectileType = ModContent.ProjectileType<FireBall>();
            projectileSpeed = 12f;
            damage = 20;
            knockBack = 3.5f;

            AddMagicSchool<FireMagic>();
        }

        public override void OnCasting(Player player, Vector2 velocity)
        {
            if (player.whoAmI != Main.myPlayer) return;

            float offsetFactor = 100 - MathHelper.Clamp(player.Center.Distance(Main.MouseWorld), 0, 100);
             
            float maxOffset = MathF.PI / (3f + offsetFactor / 5);
            velocity = velocity.RotatedBy(Main.rand.NextFloat(-maxOffset, maxOffset));
            SpawnProjectile(player, velocity);
        }
    }
}
