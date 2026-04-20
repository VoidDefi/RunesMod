using Microsoft.Xna.Framework;
using RunesMod.CastingAnimations;
using RunesMod.MagicSchools.Elemental;
using RunesMod.Projectiles.Magic.Elements.Earth;
using Terraria;
using Terraria.ModLoader;

namespace RunesMod.Spells.Elements.Earth
{
    public class GemsSpell : Spell
    {
        protected override void SetDefaults()
        {
            costType = CostTypes.Mana;
            cost = 20;

            cooldownTime = 20;
            animationTime = 20;
            animationStyle = CastingAnimationLoader.AnimationType<PushAnimation>();

            projectileType = ModContent.ProjectileType<Stalactite>();
            projectileSpeed = 13f;
            damage = 25;
            knockBack = 8.5f;

            AddMagicSchool<EarthMagic>();
        }

        public override void OnCasting(Player player, Vector2 velocity)
        {
            base.OnCasting(player, velocity);
        }
    }
}
