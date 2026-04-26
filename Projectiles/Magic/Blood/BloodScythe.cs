using System;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using RunesMod.ModUtils;
using RunesMod.DamageClasses;
using RunesMod.MagicSchools.Other;
using RunesMod.Dusts.UnitedDusts;
using RunesMod.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using ReLogic.Content;
using Terraria.Graphics;
using Terraria.Map;

namespace RunesMod.Projectiles.Magic.Blood
{
    public class BloodScythe : ModProjectile, IUnitedDustDrawer
    {
        private static int MaxTimeLeft => 100 * 60;

        public override void SetStaticDefaults()
        {

        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.timeLeft = MaxTimeLeft;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 3;
            Projectile.DamageType = ModContent.GetInstance<TrueMagic>();

            Projectile.MagicSchool().Add<BloodMagic>();
        }
        public override void AI()
        {
            if (Projectile.timeLeft == MaxTimeLeft)
            {
                Projectile.scale = 0;
            }

            float length = Projectile.velocity.Length();

            Projectile.rotation += MathF.PI / (25f - length);
            Vector2 dustPosition = Projectile.position;

            int count = Main.rand.Next(1, 3);

            for (int i = 0; i < count; i++)
            {
                Dust dust = Main.dust[Dust.NewDust
                (
                    dustPosition,
                    Projectile.width,
                    Projectile.height,
                    ModContent.DustType<BloodDust>(),
                    Projectile.velocity.X / 4f,
                    Projectile.velocity.Y / 4f,
                    Scale: 1.1f
                )];

                dust.noGravity = true;
            }

            Projectile.netUpdate = true;

            if (length < 15)
            {
                Projectile.velocity *= 1.05f;
            }

            Projectile.scale += 0.1f;
            if (Projectile.scale > 1f) Projectile.scale = 1f;
        }

        public override void OnKill(int timeLeft)
        {
            SoundStyle style = new SoundStyle("Terraria/Sounds/Item_10");
            SoundEngine.PlaySound(style, Projectile.Center);

            int count = Main.rand.Next(5, 16);

            for (int i = 0; i < count; i++)
            {
                Dust dust = Main.dust[Dust.NewDust
                (
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    ModContent.DustType<BloodDust>(),
                    Scale: 1.5f
                )];

                dust.noGravity = true;
                dust.velocity *= 2f;
                //dust.fadeIn = 1f;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            ModContent.GetInstance<BloodDust>().AddDrawer(this);

            return false;
        }

        public void PostDrawUnitedDusts(SpriteBatch spriteBatch, RenderTarget2D target)
        {
            Texture2D scythe = ModContent.Request<Texture2D>((GetType().Namespace + "." + GetType().Name).Replace('.', '/'), AssetRequestMode.ImmediateLoad).Value;

            Vector2 scale = new Vector2(1, 1) * 0.7f * Projectile.scale;

            spriteBatch.Draw(scythe, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, scythe.Size() / 2f, scale, 0f, 0f);
        }
    }
}
