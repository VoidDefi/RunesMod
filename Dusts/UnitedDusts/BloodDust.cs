using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using RunesMod.Graphics;
using RunesMod.ModUtils;
using Terraria;
using Terraria.ID;
using System;

namespace RunesMod.Dusts.UnitedDusts
{
    public class BloodDust : UnitedDust
    {
        private static readonly AutoAsset<Texture2D> circle = new AutoAsset<Texture2D>(ModAssets.Textures, "Circle");

        private static RenderTarget2D pixelTarget = null;

        private static Vector4 MainColor => new Color(201, 0, 0).ToVector4() * 1.7f;

        private static Vector4 ShadowColor => new Color(42, 0, 0).ToVector4() * 0.5f;

        private static Vector4 LightColor => new Color(255, 47, 47).ToVector4() * 4;

        private static float OutlineFactor => 0.2f;

        public override void SetStaticDefaults()
        {
            UpdateType = DustID.Stone;
        }

        public override DrawingData? GetDustDrawingData()
        {
            DrawingData data = DrawingData.WorldDrawing;

            return data;
        }

        public override DrawingData? GetCanvasDrawingData()
        {
            return new DrawingData(samplerState: SamplerState.PointClamp);
        }

        public override RenderTarget2D CreateRenderTarget()
        {
            GraphicsDevice device = Main.graphics.GraphicsDevice;

            RenderTarget2D target = new RenderTarget2D(device, Main.screenWidth, Main.screenHeight, false, device.PresentationParameters.BackBufferFormat, (DepthFormat)0);
            pixelTarget = new RenderTarget2D(device, Main.screenWidth / 2, Main.screenHeight / 2);

            return target;
        }

        public override void Draw(SpriteBatch spriteBatch, Dust dust)
        {
            if (!dust.noLight)
            {
                float factor = 10f * Math.Clamp(dust.scale, 0.01f, 1f);

                Lighting.AddLight(dust.position, ShadowColor.X * factor, ShadowColor.Y * factor, ShadowColor.Z * factor);
            }

            Vector2 defaultScale = new Vector2(1 - Math.Clamp(dust.velocity.Length() / 10, 0f, 0.8f), 1 + dust.velocity.Length() / 2);
            //Vector2 defaultScale = Vector2.One;

            Vector2 scale = defaultScale / 256f * 8f * dust.scale;

            float minScale = 0.01f;

            if (scale.X < minScale) scale.X = minScale;
            if (scale.Y < minScale) scale.Y = minScale;

            float angle = dust.velocity.AngleFrom(Vector2.Zero) + MathF.PI / 2;
            //float angle = 0;

            spriteBatch.Draw(circle.Value, dust.position - Main.screenPosition, null, Color.White, angle, Vector2.One * 256f / 2f, scale, 0, 0);
        }

        public override void PostDrawDusts(SpriteBatch spriteBatch, RenderTarget2D target)
        {
            GraphicsDevice device = Main.graphics.GraphicsDevice;

            spriteBatch.End();

            device.SetRenderTarget(pixelTarget);
            device.Clear(Color.Transparent);

            DrawingData data = new DrawingData(samplerState: SamplerState.PointClamp);
            data.Begin(spriteBatch);

            spriteBatch.Draw(target, Vector2.Zero, Color.White, 0, 0.5f / Main.GameZoomTarget);
        }

        public override void DrawCanvas(SpriteBatch spriteBatch, RenderTarget2D target)
        {
            float scale = Main.GameZoomTarget * 2;

            float targetScale = 2f * Main.GameZoomTarget;

            Effect bloodEffect = ModAssets.Request<Effect>(ModAssets.Effects, "Blood").Value;
            Texture2D shadowTexture = ModAssets.Request<Texture2D>(ModAssets.NoiseTextures, "WormNoise").Value;
            Texture2D lightTexture = ModAssets.Request<Texture2D>(ModAssets.NoiseTextures, "SimpleBubbleNoise0").Value;

            bloodEffect.Parameters["color"]?.SetValue(MainColor * OutlineFactor);
            bloodEffect.Parameters["shadowColor"]?.SetValue(ShadowColor * OutlineFactor);
            bloodEffect.Parameters["lightColor"]?.SetValue(LightColor * OutlineFactor);

            bloodEffect.Parameters["time"]?.SetValue((float)Main.gameTimeCache.TotalGameTime.TotalSeconds / 4f);
            bloodEffect.Parameters["size"]?.SetValue(Main.ScreenSize.ToVector2());

            spriteBatch.End();

            GraphicsDevice device = Main.graphics.GraphicsDevice;

            device.Textures[1] = shadowTexture;
            device.SamplerStates[1] = SamplerState.LinearWrap;
            device.Textures[2] = lightTexture;
            device.SamplerStates[2] = SamplerState.LinearWrap;

            DrawingData data = GetCanvasDrawingData() ?? new DrawingData(samplerState: SamplerState.PointClamp);
            data.Effect = bloodEffect;
            data.Begin(spriteBatch);

            spriteBatch.Draw(pixelTarget, Vector2.UnitX * -1 * scale, Color.White, targetScale);
            spriteBatch.Draw(pixelTarget, Vector2.UnitX * 1 * scale, Color.White, targetScale);
            spriteBatch.Draw(pixelTarget, Vector2.UnitY * -1 * scale, Color.White, targetScale);
            spriteBatch.Draw(pixelTarget, Vector2.UnitY * 1 * scale, Color.White, targetScale);

            spriteBatch.End();

            bloodEffect.Parameters["color"]?.SetValue(MainColor);
            bloodEffect.Parameters["shadowColor"]?.SetValue(ShadowColor);
            bloodEffect.Parameters["lightColor"]?.SetValue(LightColor);

            data.Begin(spriteBatch);

            spriteBatch.Draw(pixelTarget, Vector2.Zero, Color.White, targetScale);
        }
    }
}
