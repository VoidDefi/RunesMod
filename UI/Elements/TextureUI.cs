using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using RunesMod.ModUtils;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace RunesMod.UI.Elements
{
    public class TextureUI : UIElement
    {
        private Asset<Texture2D> texture;

        public float Scale = 1f;
        public bool ScaleToFit;

        public float Rotation;

        public bool AllowResizingDimensions = true;
        public Color Color = Color.White;
        public Vector2 NormalizedOrigin = Vector2.Zero;
        public bool RemoveFloatingPointsFromDrawPosition;

        private Texture2D nonReloadingTexture;

        public bool Hide = false;

        public bool CustomBegin = false;
        public DrawingData DrawBeginData = DrawingData.UIPointDrawing;

        public Rectangle? Frame = null;

        public TextureUI(Asset<Texture2D> texture)
        {
            SetImage(texture);
        }

        public TextureUI(Texture2D nonReloadingTexture)
        {
            SetImage(nonReloadingTexture);
        }

        public void SetImage(Asset<Texture2D> texture)
        {
            this.texture = texture;
            nonReloadingTexture = null;
            if (AllowResizingDimensions)
            {
                Width.Set(this.texture.Width(), 0f);
                Height.Set(this.texture.Height(), 0f);
            }
        }

        public void SetImage(Texture2D nonReloadingTexture)
        {
            texture = null;
            this.nonReloadingTexture = nonReloadingTexture;
            if (AllowResizingDimensions)
            {
                Width.Set(this.nonReloadingTexture.Width, 0f);
                Height.Set(this.nonReloadingTexture.Height, 0f);
            }
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            if (Frame != null)
            {
                if (Width.Pixels != Frame.Value.Width || Height.Pixels != Frame.Value.Height)
                {
                    Width.Set(Frame.Value.Width, 0f);
                    Height.Set(Frame.Value.Height, 0f);
                }
            }

            CalculatedStyle dimensions = GetDimensions();
            Texture2D texture2D = null;
            if (texture != null)
                texture2D = texture.Value;

            if (nonReloadingTexture != null)
                texture2D = nonReloadingTexture;

            if (ScaleToFit)
            {
                spriteBatch.Draw(texture2D, dimensions.ToRectangle(), Frame, Color);
                return;
            }

            Vector2 vector = texture2D.Size();
            Vector2 vector2 = dimensions.Position() + vector * (1f - Scale) / 2f + vector * NormalizedOrigin;
            if (RemoveFloatingPointsFromDrawPosition)
                vector2 = vector2.Floor();

            if (CustomBegin)
            {
                spriteBatch.End();
                DrawBeginData.Begin(spriteBatch);
            }

            spriteBatch.Draw(texture2D, vector2, Frame, Color, Rotation, vector * NormalizedOrigin, Scale, SpriteEffects.None, 0f);

            if (CustomBegin)
            {
                spriteBatch.End();
                spriteBatch.BeginUI();
            }
        }
    }
}
