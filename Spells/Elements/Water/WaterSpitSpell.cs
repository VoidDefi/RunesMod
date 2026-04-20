using RunesMod.Projectiles.Magic.Elements.Water;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using RunesMod.CastingAnimations;
using RunesMod.MagicSchools.Elemental;

namespace RunesMod.Spells.Elements.Water
{
    public class WaterSpitSpell : Spell
    {
        protected override void SetDefaults()
        {
            costType = CostTypes.Mana;
            cost = 20;

            cooldownTime = 30;
            animationTime = 30;
            animationStyle = CastingAnimationLoader.AnimationType<PushAnimation>();

            projectileType = ModContent.ProjectileType<WaterSpit>();
            projectileSpeed = 18f / 2;
            damage = 20;
            knockBack = 6.5f;

            AddMagicSchool<WaterMagic>();
        }

        public override void OnCasting(Player player, Vector2 velocity)
        {
            SoundStyle style = new SoundStyle("Terraria/Sounds/Item_21");
            SoundEngine.PlaySound(style, player.Center);

            base.OnCasting(player, velocity);
        }
    }
}