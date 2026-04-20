using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using RunesMod.Graphics;
using Terraria.GameContent;
using System;
using System.Linq;
using Terraria.Audio;
using RunesMod.ModUtils;
using RunesMod.DamageClasses;
using RunesMod.MagicSchools.Elemental;

namespace RunesMod.Projectiles.Magic.Elements.Electric
{
    public class LightingProjectile : ModProjectile
    {
        private static int MaxTimeLeft => 100 * 60;

        private static TrailDrawer vertexStrip = new TrailDrawer();

        private int timer;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 30;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.timeLeft = MaxTimeLeft;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.DamageType = ModContent.GetInstance<TrueMagic>();

            Projectile.MagicSchool().Add<ElectricMagic>();
        }

        public override void AI()
        {
            float brightness = Math.Clamp(Projectile.velocity.Length(), 0f, 1f);

            Lighting.AddLight(Projectile.Center, 0f, 0.4f * brightness, 0.55f * brightness);

            Projectile.netUpdate = true;

            Projectile.rotation = Utils.AngleFrom(new(), Projectile.velocity);

            if (Projectile.ai[0] == 0)
            {
                if (Projectile.timeLeft < MaxTimeLeft - 60)
                    Projectile.velocity *= 0.7f;

                if (MathF.Abs(Projectile.velocity.X) < 0.01f && MathF.Abs(Projectile.velocity.Y) < 0.01f)
                    Projectile.Kill();

                Projectile.position += new Vector2(Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(-1, 1)) * (Projectile.velocity.Length());
            }

            else
            {
                Projectile.velocity *= -0.5f;
            }
        }

        public override void OnKill(int timeLeft)
        {
            
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            SoundStyle style = new SoundStyle("Terraria/Sounds/Dig_0");
            SoundEngine.PlaySound(style, Projectile.Center);

            if (Projectile.ai[0] == 1) return false;

            Projectile.velocity = oldVelocity;
            Projectile.damage = 0;

            Projectile.ai[0] = 1;
            Projectile.timeLeft = 30;
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            //target.AddBuff(BuffID.Electrified, 600);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            //target.AddBuff(BuffID.Electrified, 600);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Vector4 screenRectangle = new Vector4(0, 0, Main.Camera.ScaledSize.X, Main.Camera.ScaledSize.Y);

            MiscShaderData trailShader = GameShaders.Misc["RunesMod:Trail"];
            trailShader.UseImage0(TextureAssets.Extra[ExtrasID.RainbowRodTrailShape]);
            trailShader.UseImage1(TextureAssets.Extra[ExtrasID.RainbowRodTrailShape]);
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

        Color StripColors(float progressOnStrip)
        {
            float amount = Utils.GetLerpValue(1f, 0f, progressOnStrip * 0.5f, true);
            float factor = (1f - Utils.GetLerpValue(0f, 1f, progressOnStrip)) * 2f;

            Color result = Color.Lerp(new Color(0, 0, 255), new Color(98, 208, 255), amount) * factor;
            result.A /= 2;

            float length = Projectile.velocity.Length();

            if (length <= 1f)
                result *= length;

            return result;
        }

        float StripWidth(float progressOnStrip)
        {
            return MathF.Cos((progressOnStrip - 0.5f) * MathF.PI) * 15f;
        }
    }
}
