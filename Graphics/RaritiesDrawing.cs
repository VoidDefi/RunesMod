using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using RunesMod.ModUtils;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using static System.Net.Mime.MediaTypeNames;

namespace RunesMod.Graphics
{
    public class RaritiesDrawing : GlobalItem
    {
        public override void PostDrawTooltipLine(Item item, DrawableTooltipLine line)
        {
            Vector2 LinePos = new(line.OriginalX, line.OriginalY);
            Vector2 LineCenter = LinePos + new Vector2(8 * line.Text.Length / 2, 8);

            if (line.Name == "ItemName")
            {
                if (item.rare == ModContent.GetInstance<Rarities.VoidRarity>().Type)
                {
                    Texture2D BgTexture = Mod.Assets.Request<Texture2D>("Assets/Textures/BigLightSphere").Value;
                    Color voidColor = new Color(217, 22, 220) * (5f + (float)Math.Sin(Main.GameUpdateCount / 15f) / 10);

                    Color BgColor = voidColor;
                    BgColor.A = 1;
                    Vector2 RotDist = new(1.2f + (float)(Math.Sin(Main.GameUpdateCount / 15f) * 0.5f), 1.2f + (float)(Math.Sin(Main.GameUpdateCount / 15f) * 0.5f)); //Дистанция поворота

                    DynamicSpriteFont font = FontAssets.MouseText.Value;
                    Vector2 size = (font.MeasureString(line.Text + "    ") / BgTexture.Width);

                    Vector2 scale = new Vector2((1f + (float)(Math.Sin(Main.GameUpdateCount / 15f) * 0.2f)) * (size.X + 0.3f), 1f / 256f * 25);
                    Color BgTextureColor = voidColor * 0.9f;

                    Effect overlapping = ModContent.Request<Effect>("RunesMod/Assets/Effects/Overlapping", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

                    Vector4 bgTextureColdColor = BgTextureColor.ToVector4();
                    bgTextureColdColor.Z += 0.5f;
                    bgTextureColdColor.W = 0.2f;

                    overlapping.Parameters["uColor"].SetValue(bgTextureColdColor);
                    overlapping.Parameters["uHotColor"].SetValue(BgTextureColor.ToVector4());
                    overlapping.Parameters["uHotFactor"].SetValue(1f);

                    Main.spriteBatch.End();
                    Main.spriteBatch.BeginUI(effect: overlapping);

                    Main.spriteBatch.Draw(BgTexture, LineCenter, null, BgTextureColor, 0f, new Vector2(BgTexture.Width / 2.2f, BgTexture.Height / 3.5f), scale, SpriteEffects.None, 0f);

                    Main.spriteBatch.End();
                    Main.spriteBatch.BeginUI();

                    Utils.DrawBorderString(Main.spriteBatch, line.Text, LinePos + RotDist.RotatedBy(Main.GameUpdateCount / 15f + MathHelper.Pi), BgColor, 1f);
                    Utils.DrawBorderString(Main.spriteBatch, line.Text, LinePos + RotDist.RotatedBy(Main.GameUpdateCount / 15f + MathHelper.PiOver2), BgColor, 1f);
                    Utils.DrawBorderString(Main.spriteBatch, line.Text, LinePos + RotDist.RotatedBy(Main.GameUpdateCount / 15f - MathHelper.PiOver2), BgColor, 1f);
                    Utils.DrawBorderString(Main.spriteBatch, line.Text, LinePos + RotDist.RotatedBy(Main.GameUpdateCount / 15f), BgColor, 1f);

                    Utilities.DrawStringWithColorBorder(Main.spriteBatch, line.Text, LinePos, Color.Black, voidColor, 1f);
                }
            }
        }
    }
}
