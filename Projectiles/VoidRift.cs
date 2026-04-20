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
using System.Collections.Generic;
using Terraria.DataStructures;
using RunesMod.Items.Materials;

namespace RunesMod.Projectiles
{
    public class VoidRift : ModProjectile
    {
        private static TrailDrawer vertexStrip = new TrailDrawer();
        private static int maxTimeLeft = 5 * 60;

        private static float MaxRiftThickness = 8;

        private int MaxRiftLength
        {
            get => (int)Projectile.ai[1];
            set { Projectile.ai[1] = value; }
        }

        private float RiftThickness
        {
            get => Projectile.ai[2];
            set { Projectile.ai[2] = value; }
        }

        private bool Dropped
        {
            get => Projectile.ai[0] == 1;
            set { Projectile.ai[0] = value ? 1 : 0; }
        }

        private List<Vector2> riftPositions = new();
        private List<float> riftAngles = new();

        public override void SetStaticDefaults()
        {

        }

        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.timeLeft = maxTimeLeft;
            Projectile.tileCollide = false;
            Projectile.friendly = false;
            Projectile.hostile = false;
        }

        public override void OnSpawn(IEntitySource source)
        {
            float factor = Main.rand.NextFloat(0.6f, 1.4f);

            float size = 32f * factor;
            MaxRiftLength = (int)(30f * factor);
            RiftThickness = 2f;

            Vector2 center = Projectile.position;
            Projectile.position -= Vector2.One * size;

            Projectile.position.X += Main.rand.NextFloat(0f, 1f) * size * 2;

            if (Main.rand.NextBool())
                Projectile.position.Y += size * 2;

            Projectile.velocity = -center.DirectionTo(Projectile.position);
            Projectile.velocity *= (size * 2f + 8f) / (float)MaxRiftLength;
        }

        public override void AI()
        {
            float factor = RiftThickness / MaxRiftThickness;
            Lighting.AddLight(Projectile.Center, 0.35f * factor, 0.01f * factor, 0.4f * factor);

            Projectile.netUpdate = true;

            if (maxTimeLeft - Projectile.timeLeft >= MaxRiftLength)
            {
                Projectile.velocity = Vector2.Zero;

                RiftThickness = MathHelper.Lerp(RiftThickness, MaxRiftThickness, 0.1f);
            }

            else
            {
                riftPositions.Add(Projectile.position);
                riftAngles.Add(Vector2.Zero.AngleFrom(Projectile.velocity));
            }

            if (Projectile.timeLeft <= 1)
            {
                RiftThickness = MathHelper.Lerp(RiftThickness, -MaxRiftThickness, 0.1f);

                if (RiftThickness > 0.001f) Projectile.timeLeft = 2;
            }

            if (Projectile.timeLeft <= 10 && !Dropped)
            {
                Item.NewItem(new EntitySource_Misc("FallingStar"), riftPositions[riftPositions.Count / 2], ModContent.ItemType<VoidFragment>());
                Dropped = true;
            }
        }

        public override void OnKill(int timeLeft)
        {
            
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Color borderColor = new Color(177, 12, 174) * 0.6f;
            borderColor.A = 255;

            float size = 3f;
            float border = 1.2f;
            Vector2 textureOffset = new Vector2(0.01f, 0);

            Vector4 screenRectangle = new Vector4(0, 0, Main.Camera.ScaledSize.X, Main.Camera.ScaledSize.Y);
            Vector2 translation = Main.ScreenSize.ToVector2();

            Vector2[] oldPos = riftPositions.ToArray();
            float[] oldRot = riftAngles.ToArray();
            Vector2 offset = -Main.screenPosition + Projectile.Size / 2f;
            
            MiscShaderData shader = GameShaders.Misc["RunesMod:StaticTrail"];

            //Draw OutLine

            shader.UseImage0(ModAssets.Request<Texture2D>(ModAssets.Textures, "Pixel"));

            shader.Shader.Parameters["uWorldViewProjection"]?.SetValue(Main.GameViewMatrix.NormalizedTransformationmatrix);
            shader.Shader.Parameters["uSize"].SetValue(Vector2.One);
            shader.Shader.Parameters["uImageOffset0"].SetValue(Vector2.Zero);
            shader.Apply();

            vertexStrip.PrepareStripWithProceduralPadding(oldPos, oldRot, _ => borderColor, p => StripWidth(p) * border, offset);
            vertexStrip.DrawTrail();

            //Draw Inside

            shader.UseImage0(ModAssets.Request<Texture2D>(ModAssets.NoiseTextures, "SpaceNoise"));

            shader.Shader.Parameters["uWorldViewProjection"]?.SetValue(Main.GameViewMatrix.NormalizedTransformationmatrix);
            shader.Shader.Parameters["uSize"].SetValue(size * translation / Math.Max(translation.X, translation.Y));
            shader.Shader.Parameters["uImageOffset0"].SetValue(textureOffset * size);
            shader.Apply();

            List<Vector2> insidePos = oldPos.ToList();
            List<float> insideRot = oldRot.ToList();

            insidePos.RemoveAt(0);
            insideRot.RemoveAt(0);

            if (insidePos.Count >= 1 && insideRot.Count >= 1)
            {
                insidePos.RemoveAt(insidePos.Count - 1);
                insideRot.RemoveAt(insideRot.Count - 1);

                vertexStrip.PrepareStripWithProceduralPadding(insidePos.ToArray(), insideRot.ToArray(), _ => new Color(180, 20, 225) * 2f, StripWidth, offset);
                vertexStrip.DrawTrail();
            }

            Main.pixelShader.CurrentTechnique.Passes[0].Apply();

            return true;
        }

        public override void PostDraw(Color lightColor)
        {
            
        }

        float StripWidth(float progressOnStrip)
        {
            return (0 - Math.Abs(progressOnStrip * 2 - 1) + 1) * RiftThickness * riftPositions.Count / MaxRiftLength;
        }
    }
}
