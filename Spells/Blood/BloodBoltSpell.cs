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
    public class BloodBoltSpell : Spell
    {
        protected override void SetDefaults()
        {
            costType = CostTypes.Life;
            cost = 5;
            bloodCost = 100f;

            cooldownTime = 30;
            animationTime = 30;
            animationStyle = CastingAnimationLoader.AnimationType<PushAnimation>();

            projectileType = ModContent.ProjectileType<BloodBolt>();
            projectileSpeed = 18f / 2;
            damage = 30;
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