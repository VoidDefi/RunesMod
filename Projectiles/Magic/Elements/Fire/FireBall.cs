using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.Graphics;
using Terraria.ID;
using Terraria.ModLoader;
using RunesMod.Graphics;
using Microsoft.CodeAnalysis.Text;
using Terraria.GameContent;
using System;
using System.Threading;
using System.Linq;
using Terraria.Audio;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RunesMod.ModUtils;
using RunesMod.DamageClasses;
using RunesMod.MagicSchools.Elemental;

namespace RunesMod.Projectiles.Magic.Elements.Fire
{
    public class FireBall : ModProjectile
    {
        private static int maxTimeLeft = 10 * 60;
        private static TrailDrawer vertexStrip = new TrailDrawer();

        private int timer;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 30;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.damage = 10;
            Projectile.timeLeft = maxTimeLeft;
            Projectile.tileCollide = true;
            Projectile.knockBack = 0.2f;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = ModContent.GetInstance<TrueMagic>();

            Projectile.MagicSchool().Add<FireMagic>();
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 1f, 0.4f, 0f);

            foreach (var pos in Projectile.oldPos.Append(Projectile.position))
            {
                int dustCount = Main.rand.Next(-180, 3);

                for (int i = 0; i < dustCount; i++)
                {
                    float scale = Main.rand.NextFloat(0.5f, 2.5f);

                    Dust dust;
                    dust = Main.dust[Dust.NewDust(pos, Projectile.width, Projectile.height, 6, 0f, 0f, 0, new Color(255, 255, 255), scale)];

                    if (Main.rand.Next(0, 10) > 6)
                    {
                        dust.velocity = Vector2.Zero;
                        dust.noGravity = true;
                    }
                }
            }

            float mouseDistance = Main.player[Main.myPlayer].Distance(Main.MouseWorld);

            if (!Projectile.netUpdate && Projectile.owner == Main.myPlayer && Projectile.ai[0] == 0f)
                if (mouseDistance < Main.player[Main.myPlayer].Distance(Projectile.Center))
                    Projectile.ai[0] = 1f;

            if (Projectile.ai[0] < 1f)
            {
                Vector2 direction = Projectile.Center.DirectionTo(Main.MouseWorld);
                Projectile.velocity += direction;

                float length = Projectile.velocity.Length();

                Projectile.velocity.Normalize();
                Projectile.velocity *= length * 0.95f + 0.15f * (50f - MathHelper.Clamp(mouseDistance, 0, 50f));
            }

            else Projectile.netUpdate = true;

            Tile tile = Main.tile[(Projectile.Center / 16).ToPoint()];

            if (tile.LiquidAmount > 0 && tile.LiquidType != LiquidID.Lava)
            {
                Projectile.ai[1] = 1;
                Projectile.Kill();
            }
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);

            Vector2 dustDirection = Vector2.One * Projectile.velocity.Length() / 2f;

            Vector2[] oldPositions = Projectile.oldPos;
            oldPositions.Append(Projectile.position);

            for (int i = 0; i < oldPositions.Length; i++)
            {
                if (Main.rand.Next(0, 10) <= 6) continue;

                Vector2 pos = oldPositions[i];
                float scale = Main.rand.NextFloat(0.5f, 1f) * (oldPositions.Length - i) / 7;

                if (i < oldPositions.Length - 1)
                {
                    Vector2 current = oldPositions[i];
                    Vector2 next = oldPositions[i + 1];

                    Dust dust;
                    dust = Main.dust[Dust.NewDust(pos, Projectile.width, Projectile.height, 6, 0f, 0f, 0, new Color(255, 255, 255), scale)];
                    dust.noGravity = true;
                    dust.velocity = dustDirection.RotatedBy(current.AngleFrom(next) - MathHelper.PiOver4);
                }
            }

            if (Projectile.ai[1] != 1)
            {

                int dustCount = (int)(Main.rand.Next(5, 18) * Projectile.velocity.Length() / 20);
                int realDustCount = dustCount + Main.rand.Next(2, 7);

                float offsetAngle = 0.05f;
                float addedAngle = MathHelper.ToRadians(1f / dustCount * 360f);
                float angle = Main.rand.NextFloat(-offsetAngle, offsetAngle);

                for (int i = 0; i < realDustCount; i++)
                {
                    float scale = Main.rand.NextFloat(1f, 3f);

                    Dust dust;
                    dust = Main.dust[Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, 6, 0f, 0f, 0, new Color(255, 255, 255), scale)];
                    dust.velocity = (dustDirection * Main.rand.NextFloat(0.1f, 0.3f)).RotatedBy(angle) + Projectile.velocity / 5;

                    if (Main.rand.Next(0, 10) > 6)
                        dust.noGravity = true;

                    angle += addedAngle + Main.rand.NextFloat(-offsetAngle, offsetAngle);
                }
            }

            else
            {
                int dustCount = Main.rand.Next(5, 18);
                int realDustCount = dustCount + Main.rand.Next(2, 7);

                float offsetAngle = 0.05f;
                float addedAngle = MathHelper.ToRadians(1f / dustCount * 360f);
                float angle = Main.rand.NextFloat(-offsetAngle, offsetAngle);

                for (int i = 0; i < realDustCount; i++)
                {
                    int type = Main.rand.Next(GoreID.Smoke1, GoreID.Smoke3 + 1);
                    float scale = Main.rand.NextFloat(0.5f, 1f);

                    Vector2 velocity = (dustDirection * Main.rand.NextFloat(0.1f, 0.3f)).RotatedBy(angle);

                    Gore.NewGore(Projectile.GetSource_Death(), Projectile.Center, velocity, type, scale);

                    angle += addedAngle + Main.rand.NextFloat(-offsetAngle, offsetAngle);
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Vector4 screenRectangle = new Vector4(0, 0, Main.Camera.ScaledSize.X, Main.Camera.ScaledSize.Y);

            MiscShaderData trailShader = GameShaders.Misc["RunesMod:Trail"];
            trailShader.UseImage0(TextureAssets.Extra[ExtrasID.RainbowRodTrailShape]);
            trailShader.UseImage1(TextureAssets.Extra[ExtrasID.MagicMissileTrailShape]);
            trailShader.UseColor(Color.Orange);
            trailShader.UseOpacity(2f);

            trailShader.Shader.Parameters["uWorldViewProjection"]?.SetValue(Main.GameViewMatrix.NormalizedTransformationmatrix);
            trailShader.Apply();

            Vector2[] oldPos = Projectile.oldPos;
            float[] oldRot = Projectile.oldRot;
            Vector2 offset = -Main.screenPosition + Projectile.Size / 2f;

            vertexStrip.PrepareStripWithProceduralPadding(oldPos, oldRot, StripColors, StripWidth, offset);
            vertexStrip.DrawTrail();

            Main.pixelShader.CurrentTechnique.Passes[0].Apply();

            return true;
        }

        public override void PostDraw(Color lightColor)
        {
            Effect overlapping = ModAssets.Request<Effect>(ModAssets.Effects, "Overlapping").Value;
            Texture2D lightSphere = ModAssets.Request<Texture2D>(ModAssets.Textures, "BigLightSphere").Value;
            Texture2D ballMask = ModAssets.Request<Texture2D>(ModAssets.Textures, "BallMask").Value;

            Main.spriteBatch.End();

            overlapping.Parameters["uColor"].SetValue(new Vector4(1.5f, 0.2f, 0, 0.1f) * 1.5f);
            overlapping.Parameters["uHotColor"].SetValue(new Vector4(1.2f, 1f, 0, 0.1f) * 1.5f);
            overlapping.Parameters["uHotFactor"].SetValue(3.5f);

            Main.spriteBatch.Begin(overlapping);

            Vector2 drawPos = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);

            Vector2 sphereOrigin = new Vector2(lightSphere.Width * 0.5f, lightSphere.Height * 0.5f);
            Main.EntitySpriteDraw(lightSphere, drawPos, null, Color.White, 0, sphereOrigin, (16f + MathF.Sin(timer / 10f)) / 75f, SpriteEffects.None, 0);

            Main.spriteBatch.End();

            overlapping.Parameters["uColor"].SetValue(new Vector4(1.5f, 0f, 0f, 1f) * 4f);
            overlapping.Parameters["uHotColor"].SetValue(new Vector4(1.3f, 1.3f, 0f, 1f) * 2f);
            overlapping.Parameters["uHotFactor"].SetValue(1f);

            Main.spriteBatch.Begin(overlapping);

            float rotation = -(timer / 20f * Projectile.velocity.Length()) % (MathF.PI * 10f * 2f);
            Vector2 ballOrigin = new Vector2(ballMask.Width * 0.5f, ballMask.Height * 0.5f);
            Main.EntitySpriteDraw(ballMask, drawPos, null, Color.White, rotation, ballOrigin, 1f, SpriteEffects.None, 0);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(null);

            timer++;
        }

        Color StripColors(float progressOnStrip)
        {
            float amount = Utils.GetLerpValue(1f, 0f, progressOnStrip * 0.5f, true);
            float factor = (1f - Utils.GetLerpValue(0f, 1f, progressOnStrip)) * 2f;

            Color result = Color.Lerp(new Color(255, 0, 0), Color.Orange, amount) * factor;
            result.A /= 2;
            return result;
        }

        float StripWidth(float progressOnStrip)
        {
            float factor = Utils.GetLerpValue(0f, 0.05f, progressOnStrip, true);

            return MathHelper.Lerp(32f, 26f, progressOnStrip) * factor;
        }
    }
}
