using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using Terraria.GameContent;
using Terraria.UI.Chat;

namespace RunesMod.ModUtils
{
    public static class Utilities
    {
        public static Vector2 DrawStringWithColorBorder(SpriteBatch sb, string text, Vector2 pos, Color color, Color borderColor, float scale = 1f, float anchorx = 0f, float anchory = 0f, int maxCharactersDisplayed = -1)
        {
            if (maxCharactersDisplayed != -1 && text.Length > maxCharactersDisplayed)
            {
                text.Substring(0, maxCharactersDisplayed);
            }

            DynamicSpriteFont value = FontAssets.MouseText.Value;
            Vector2 vector = value.MeasureString(text);
            DrawColorCodedStringWithColorShadow(sb, value, text, pos, color, borderColor, 0f, new Vector2(anchorx, anchory) * vector, new Vector2(scale), -1f, 1.5f);
            return vector * scale;
        }

        public static Vector2 DrawColorCodedStringWithColorShadow(SpriteBatch spriteBatch, DynamicSpriteFont font, string text, Vector2 position, Color baseColor, Color shadowColor, float rotation, Vector2 origin, Vector2 baseScale, float maxWidth = -1f, float spread = 2f)
        {
            TextSnippet[] snippets = ChatManager.ParseMessage(text, baseColor).ToArray();
            TextSnippet[] shadowSnippets = ChatManager.ParseMessage(text, shadowColor).ToArray();
            ChatManager.ConvertNormalSnippets(snippets);
            ChatManager.DrawColorCodedStringShadow(spriteBatch, font, shadowSnippets, position, shadowColor, rotation, origin, baseScale, maxWidth, spread);
            int hoveredSnippet;
            return ChatManager.DrawColorCodedString(spriteBatch, font, snippets, position, Color.White, rotation, origin, baseScale, out hoveredSnippet, maxWidth);
        }

        public static Color Sub(this Color left, Color right)
        {
            Vector4 v = left.ToVector4() - right.ToVector4();
            return new Color(v);
        }

        public static Vector2 Curve(Vector2 start, Vector2 center, Vector2 end, float amount)
        {
            Vector2 startLerp = new Vector2(MathHelper.Lerp(start.X, center.X, amount), MathHelper.Lerp(start.Y, center.Y, amount));
            Vector2 endLerp = new Vector2(MathHelper.Lerp(center.X, end.X, amount), MathHelper.Lerp(center.Y, end.Y, amount));
            Vector2 curve = new Vector2(MathHelper.Lerp(startLerp.X, endLerp.X, amount), MathHelper.Lerp(startLerp.Y, endLerp.Y, amount));

            return curve;
        }
    }
}
