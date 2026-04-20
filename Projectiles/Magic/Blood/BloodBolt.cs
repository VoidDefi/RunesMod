using RunesMod.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using RunesMod.ModUtils;
using RunesMod.MagicSchools.Elemental;
using RunesMod.DamageClasses;
using RunesMod.MagicSchools.Other;

namespace RunesMod.Projectiles.Magic.Blood
{
    public class BloodBolt : ModProjectile
    {
        private static int MaxTimeLeft => 100 * 60;
        private static TrailDrawer vertexStrip = new TrailDrawer();

        private bool Collide
        {
            get => Projectile.ai[0] == 1;
            set { Projectile.ai[0] = value ? 1 : 0; }
        }

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 30 * 2;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 30 * 2;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 3;
            Projectile.extraUpdates = 2;

            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.timeLeft = MaxTimeLeft;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.DamageType = ModContent.GetInstance<TrueMagic>();

            Projectile.MagicSchool().Add<BloodMagic>();
        }

        public override void AI()
        {
            float brightness = Math.Clamp(Projectile.velocity.Length(), 0f, 1f);
            Lighting.AddLight(Projectile.Center, 1f * brightness, 0f, 0f);

            Projectile.netUpdate = true;

            if (!Collide)
            {
                //Projectile.velocity.Y += 0.05f * Projectile.extraUpdates;
            }

            else
            {
                Projectile.velocity *= 0.8f;

                if (Projectile.velocity.Length() < 0.00001f)
                    Projectile.Kill();
            }
        }

        public override void OnKill(int timeLeft)
        {
            
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (!Collide)
            {
                SoundStyle style = new SoundStyle("Terraria/Sounds/Item_10");
                SoundEngine.PlaySound(style, Projectile.Center);
            }

            Collide = true;
            Projectile.damage = 0;

            Projectile.velocity = oldVelocity;

            float maxLength = 0;

            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                float length = Projectile.oldPos[i].Length();
                maxLength = MathF.Max(maxLength, length);
            }

            return maxLength < 0.01f;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {

        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {

        }
        int timer;
        public override bool PreDraw(ref Color lightColor)
        {
            Vector4 screenRectangle = new Vector4(0, 0, Main.Camera.ScaledSize.X, Main.Camera.ScaledSize.Y);

            MiscShaderData trailShader = GameShaders.Misc["RunesMod:Trail"];

            trailShader.UseImage0(ModAssets.Request<Texture2D>(ModAssets.Textures, "Pixel"));
            trailShader.UseImage1(TextureAssets.Extra[ExtrasID.RainbowRodTrailShape]);//ModAssets.Request<Texture2D>(ModAssets.NoiseTextures, "FoamNoise"));

            trailShader.Shader.Parameters["uWorldViewProjection"]?.SetValue(Main.GameViewMatrix.NormalizedTransformationmatrix);
            trailShader.Apply();

            Vector2[] oldPos = Projectile.oldPos;
            float[] oldRot = Projectile.oldRot.ToList().ToArray();
            Vector2 offset = -Main.screenPosition + Projectile.Size / 2f;

            vertexStrip.PrepareStripWithProceduralPadding(oldPos, oldRot, StripColors, StripWidth, offset);
            vertexStrip.DrawTrail();

            Main.pixelShader.CurrentTechnique.Passes[0].Apply();

            timer++;

            return true;
        }

        Color StripColors(float progressOnStrip)
        {
            float amount = Utils.GetLerpValue(1f, 0f, progressOnStrip * 0.5f, true);
            float factor = (1f - Utils.GetLerpValue(0f, 1f, progressOnStrip)) * 2f;
            factor /= 2;

            Color result = Color.Lerp(new Color(0, 0, 0), new Color(255, 47, 47), amount) * factor;
            result.A = (byte)(result.A * 0.8f);

            float length = Projectile.velocity.Length();
            if (length <= 1f) result *= length;

            return result;
        }

        float StripWidth(float progressOnStrip)
        {
            float x = progressOnStrip;
            return (MathF.Cos((x - 0.55f) * MathF.PI + MathF.Sin(x * MathF.PI)) * 2f) * 10f;
        }
    }
}
