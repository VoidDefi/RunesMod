using RunesMod.Projectiles.Magic.Blood;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using RunesMod.CastingAnimations;
using RunesMod.MagicSchools.Other;
using RunesMod.MagicSchools.Elemental;

namespace RunesMod.Spells.Blood
{
    public class BloodScytheSpell : Spell
    {
        protected override void SetDefaults()
        {
            costType = CostTypes.Life;
            cost = 5;
            bloodCost = 100f;

            cooldownTime = 30;
            animationTime = 30;
            animationStyle = CastingAnimationLoader.AnimationType<PushAnimation>();

            projectileType = ModContent.ProjectileType<BloodScythe>();
            projectileSpeed = 0.5f;
            damage = 50;
            knockBack = 4.5f;

            AddMagicSchool<BloodMagic>();
        }

        public override void OnCasting(Player player, Vector2 velocity)
        {
            SoundStyle style = new SoundStyle("Terraria/Sounds/Item_21");
            SoundEngine.PlaySound(style, player.Center);

            base.OnCasting(player, velocity);
        }
    }
}