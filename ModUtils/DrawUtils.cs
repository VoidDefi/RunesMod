using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using RunesMod.ModUtils.Reflection;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace RunesMod.ModUtils
{
    public static class DrawUtils
    {
        #region Fields

        private static readonly AutoFieldInfo SortModeInfo = new(typeof(SpriteBatch), "sortMode");

        private static readonly AutoFieldInfo BlendStateInfo = new(typeof(SpriteBatch), "blendState");

        private static readonly AutoFieldInfo SamplerStateInfo = new(typeof(SpriteBatch), "samplerState");

        private static readonly AutoFieldInfo DepthStencilStateInfo = new(typeof(SpriteBatch), "depthStencilState");

        private static readonly AutoFieldInfo RasterizerStateInfo = new(typeof(SpriteBatch), "rasterizerState");

        private static readonly AutoFieldInfo TransformMatrixInfo = new(typeof(SpriteBatch), "transformMatrix");

        private static readonly AutoFieldInfo CustomEffectInfo = new(typeof(SpriteBatch), "customEffect");

        #endregion

        #region Begins

        public static void Begin(this SpriteBatch spriteBatch, Effect effect, Matrix? matrix = null, bool withoutMatrix = false)
        {
            if (!withoutMatrix)
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState,
                                  DepthStencilState.None, RasterizerState.CullNone, effect,
                                  matrix == null ? Main.GameViewMatrix.TransformationMatrix : matrix.Value);

            else
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState,
                                  DepthStencilState.None, RasterizerState.CullNone, effect);
        }

        public static void BeginUI(this SpriteBatch spriteBatch, SamplerState samplerState = null, Effect effect = null)
        {
            spriteBatch.Begin
            (
                SpriteSortMode.Deferred, 
                BlendState.AlphaBlend, 
                samplerState == null ? SamplerState.LinearClamp : samplerState,
                DepthStencilState.None, 
                RasterizerState.CullCounterClockwise,
                effect, 
                Main.UIScaleMatrix
            );
        }

        public static void BeginCopy(this SpriteBatch spriteBatch, Effect effect = null, bool copyEffect = false)
        {
            spriteBatch.BeginCopy(spriteBatch, effect, copyEffect);
        }

        public static void BeginCopy(this SpriteBatch spriteBatch, SpriteBatch source, Effect effect = null, bool copyEffect = false)
        {
            spriteBatch.Begin
            (
                (SpriteSortMode)SortModeInfo.Value.GetValue(source),
                (BlendState)BlendStateInfo.Value.GetValue(source),
                (SamplerState)SamplerStateInfo.Value.GetValue(source),
                (DepthStencilState)DepthStencilStateInfo.Value.GetValue(source),
                (RasterizerState)RasterizerStateInfo.Value.GetValue(source),
                copyEffect ? (Effect)CustomEffectInfo.Value.GetValue(source) : effect,
                (Matrix)TransformMatrixInfo.Value.GetValue(source)
            );
        }

        #endregion

        #region SpriteBatch Draw

        public static void Draw(this SpriteBatch spriteBatch, Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, float scale)
        {
            spriteBatch.Draw(texture, position, sourceRectangle, color, rotation, origin, scale, 0f, 1f);
        }

        public static void Draw(this SpriteBatch spriteBatch, Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, Vector2 scale)
        {
            spriteBatch.Draw(texture, position, sourceRectangle, color, rotation, origin, scale, 0f, 1f);
        }

        public static void Draw(this SpriteBatch spriteBatch, Texture2D texture, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale)
        {
            spriteBatch.Draw(texture, position, null, color, rotation, origin, scale, 0f, 1f);
        }

        public static void Draw(this SpriteBatch spriteBatch, Texture2D texture, Vector2 position, Color color, float rotation, Vector2 origin, float scale)
        {
            spriteBatch.Draw(texture, position, null, color, rotation, origin, scale, 0f, 1f);
        }

        public static void Draw(this SpriteBatch spriteBatch, Texture2D texture, Vector2 position, Color color, float rotation, Vector2 scale)
        {
            spriteBatch.Draw(texture, position, null, color, rotation, Vector2.Zero, scale, 0f, 1f);
        }

        public static void Draw(this SpriteBatch spriteBatch, Texture2D texture, Vector2 position, Color color, float rotation, float scale)
        {
            spriteBatch.Draw(texture, position, null, color, rotation, Vector2.Zero, scale, 0f, 1f);
        }

        public static void Draw(this SpriteBatch spriteBatch, Texture2D texture, Vector2 position, Color color, Vector2 scale)
        {
            spriteBatch.Draw(texture, position, null, color, 0f, Vector2.Zero, scale, 0f, 1f);
        }

        public static void Draw(this SpriteBatch spriteBatch, Texture2D texture, Vector2 position, Color color, float scale)
        {
            spriteBatch.Draw(texture, position, null, color, 0f, Vector2.Zero, scale, 0f, 1f);
        }

        public static void Draw(this SpriteBatch spriteBatch, Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 scale)
        {
            spriteBatch.Draw(texture, position, sourceRectangle, color, rotation, Vector2.Zero, scale, 0f, 1f);
        }

        public static void Draw(this SpriteBatch spriteBatch, Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, float scale)
        {
            spriteBatch.Draw(texture, position, sourceRectangle, color, rotation, Vector2.Zero, scale, 0f, 1f);
        }

        public static void Draw(this SpriteBatch spriteBatch, Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, Vector2 scale)
        {
            spriteBatch.Draw(texture, position, sourceRectangle, color, 0f, Vector2.Zero, scale, 0f, 1f);
        }

        public static void Draw(this SpriteBatch spriteBatch, Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float scale)
        {
            spriteBatch.Draw(texture, position, sourceRectangle, color, 0f, Vector2.Zero, scale, 0f, 1f);
        }

        public static void Draw(this SpriteBatch spriteBatch, Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, Vector2 scale, SpriteEffects effects)
        {
            spriteBatch.Draw(texture, position, sourceRectangle, color, 0f, Vector2.Zero, scale, effects, 1f);
        }

        public static void Draw(this SpriteBatch spriteBatch, Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float scale, SpriteEffects effects)
        {
            spriteBatch.Draw(texture, position, sourceRectangle, color, 0f, Vector2.Zero, scale, effects, 1f);
        }

        #endregion

        #region Frame Utils

        /// <inheritdoc cref="Terraria.Utils.Frame(Texture2D, int, int, int, int, int, int)"/>
        public static Rectangle SafeFrame(this Asset<Texture2D> asset, int horizontalFrames = 1, int verticalFrames = 1, int frameX = 0, int frameY = 0, int sizeOffsetX = 0, int sizeOffsetY = 0)
        {
            Rectangle frame = asset.Frame(horizontalFrames, verticalFrames, frameX, frameY, sizeOffsetX, sizeOffsetY);
            
            if(frame.Width < asset.Width())
                frame.Width--;

            if (frame.Height < asset.Height())
                frame.Height--;

            return frame;
        }

        /// <inheritdoc cref="Terraria.Utils.Frame(Texture2D, int, int, int, int, int, int)"/>
        public static Rectangle SafeFrame(this Texture2D texture, int horizontalFrames = 1, int verticalFrames = 1, int frameX = 0, int frameY = 0, int sizeOffsetX = 0, int sizeOffsetY = 0)
        {
            Rectangle frame = texture.Frame(horizontalFrames, verticalFrames, frameX, frameY, sizeOffsetX, sizeOffsetY);

            if (frame.Width < texture.Width)
                frame.Width--;

            if (frame.Height < texture.Height)
                frame.Height--;

            return frame;
        }

        #endregion
    }
}
