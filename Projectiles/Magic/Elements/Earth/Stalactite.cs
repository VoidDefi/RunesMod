using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.Graphics;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.CodeAnalysis.Text;
using Terraria.GameContent;
using System;
using System.Threading;
using System.Linq;
using Terraria.Audio;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RunesMod.DamageClasses;
using RunesMod.MagicSchools.Elemental;
using RunesMod.ModUtils;

namespace RunesMod.Projectiles.Magic.Elements.Earth
{
    public class Stalactite : ModProjectile
    {
        private static int maxTimeLeft = 10 * 60;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            Main.projFrames[Projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.damage = 15;
            Projectile.knockBack = 0.8f;
            Projectile.timeLeft = maxTimeLeft;
            Projectile.tileCollide = true;
            Projectile.penetrate = 3;
            //Projectile.usesLocalNPCImmunity = true;
            //Projectile.localNPCHitCooldown = 12;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = ModContent.GetInstance<TrueMagic>();

            Projectile.MagicSchool().Add<EarthMagic>();
        }

        public override void OnSpawn(IEntitySource source)
        {
            Projectile.frame = Main.rand.Next(0, Main.projFrames[Projectile.type]);
            Projectile.netUpdate = true;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.velocity *= 0.5f;
        }

        public override void AI()
        {
            Projectile.velocity.Y = Projectile.velocity.Y + 0.5f;

            if (Projectile.velocity.Y > 16f)
                Projectile.velocity.Y = 16f;

            Projectile.rotation = Vector2.Zero.AngleFrom(Projectile.velocity) - MathHelper.PiOver2;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.DD2_MonkStaffGroundImpact, Projectile.position);

            Vector2 dustDirection = Vector2.One * Projectile.velocity.Length() / 2f;

            int dustCount = (int)(Main.rand.Next(5, 18) * Projectile.velocity.Length() / 10);
            int realDustCount = dustCount + Main.rand.Next(2, 7);

            float offsetAngle = 0.05f;
            float addedAngle = MathHelper.ToRadians(1f / dustCount * 360f);
            float angle = Main.rand.NextFloat(-offsetAngle, offsetAngle);

            for (int i = 0; i < realDustCount; i++)
            {
                float scale = Main.rand.NextFloat(0.5f, 2f);

                Dust dust;
                dust = Main.dust[Dust.NewDust(Projectile.oldPosition, Projectile.width, Projectile.height, 1, 0f, 0f, 0, new Color(217, 185, 156), scale)];
                dust.velocity = (dustDirection * Main.rand.NextFloat(0.1f, 0.3f)).RotatedBy(angle) + Projectile.velocity / 5 * Main.rand.Next(0, 2);

                angle += addedAngle + Main.rand.NextFloat(-offsetAngle, offsetAngle);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;

            int frameCount = Main.projFrames[Projectile.type];
            int frameHeight = texture.Height / frameCount;
            Rectangle frame = new Rectangle(0, Projectile.frame * frameHeight, texture.Width, texture.Height / frameCount);

            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
            for (int i = Projectile.oldPos.Length - 1; i > 0; i--)
            {
                Vector2 drawPos = Projectile.oldPos[i] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, frame, color, Projectile.oldRot[i], drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            }

            return true;
        }
    }
}
