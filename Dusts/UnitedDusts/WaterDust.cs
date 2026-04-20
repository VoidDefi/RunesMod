using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using RunesMod.Graphics;
using RunesMod.ModUtils;
using Terraria;
using Terraria.ID;

namespace RunesMod.Dusts.UnitedDusts
{
    public class WaterDust : UnitedDust
    {
        private readonly AutoAsset<Texture2D> circle = new AutoAsset<Texture2D>(ModAssets.Textures, "Circle");

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
            return null;
        }

        public override RenderTarget2D CreateRenderTarget()
        {
            GraphicsDevice device = Main.graphics.GraphicsDevice;

            RenderTarget2D target = new RenderTarget2D(device, Main.screenWidth, Main.screenHeight, false, device.PresentationParameters.BackBufferFormat, (DepthFormat)0);

            return target;
        }

        public override void Draw(SpriteBatch spriteBatch, Dust dust)
        {
            spriteBatch.Draw(circle.Value, dust.position - Main.screenPosition, null, Color.White, 0f, Vector2.One * 256f / 2f, 1f / 256f * 8f * dust.scale, 0, 0);
        }

        public override void DrawCanvas(SpriteBatch spriteBatch, RenderTarget2D target)
        {
            spriteBatch.Draw(target, Vector2.One * -4, null, Color.Black);
            spriteBatch.Draw(target, new Vector2(4, -4), null, Color.Black);
            spriteBatch.Draw(target, new Vector2(-4, 4), null, Color.Black);
            spriteBatch.Draw(target, Vector2.One * 4, null, Color.Black);

            spriteBatch.Draw(target, Vector2.UnitX * -4, null, Color.Black);
            spriteBatch.Draw(target, Vector2.UnitX * 4, null, Color.Black);
            spriteBatch.Draw(target, Vector2.UnitY * -4, null, Color.Black);
            spriteBatch.Draw(target, Vector2.UnitY * 4, null, Color.Black);

            spriteBatch.Draw(target, Vector2.Zero, null, Color.White);
        }
    }
}
