using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria.Achievements;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria;
using Terraria.UI;

namespace RunesMod.Systems.Knowledges
{
    public class KnowledgeNotification : IInGameNotification
    {
        private IKnowledge Knowledge { get; set; } 

        private const int _iconSize = 64;

        private const int _iconSizeWithSpace = 66;

        private const int _iconsPerRow = 8;

        private int _iconIndex;

        private int timeLeft;

        public bool ShouldBeRemoved { get; private set; }

        private float Scale
        {
            get
            {
                if (timeLeft < 30)
                {
                    return MathHelper.Lerp(0f, 1f, (float)timeLeft / 30f);
                }
                if (timeLeft > 285)
                {
                    return MathHelper.Lerp(1f, 0f, ((float)timeLeft - 285f) / 15f);
                }
                return 1f;
            }
        }

        private float Opacity
        {
            get
            {
                float scale = Scale;
                if (scale <= 0.5f)
                {
                    return 0f;
                }
                return (scale - 0.5f) / 0.5f;
            }
        }

        public KnowledgeNotification(IKnowledge knowledge)
        {
            Knowledge = knowledge;
            timeLeft = 300;
        }

        public void Update()
        {
            timeLeft--;

            if (timeLeft <= 0)
            {
                ShouldBeRemoved = true;
                timeLeft = 0;
            }
        }

        public void PushAnchor(ref Vector2 anchorPosition)
        {
            float num = 50f * Opacity;
            anchorPosition.Y -= num;
        }

        public void DrawInGame(SpriteBatch sb, Vector2 bottomAnchorPosition)
        {
            string name = Knowledge.DisplayName.Value;

            float opacity = Opacity;
            if (opacity > 0f)
            {
                float num = Scale * 1.1f;
                Vector2 size = (FontAssets.ItemStack.Value.MeasureString(name) + new Vector2(58f, 10f)) * num;
                Rectangle r = Utils.CenteredRectangle(bottomAnchorPosition + new Vector2(0f, (0f - size.Y) * 0.5f), size);
                Vector2 mouseScreen = Main.MouseScreen;
                bool num3 = r.Contains(mouseScreen.ToPoint());
                
                Utils.DrawInvBG(c: num3 ? (new Color(64, 109, 164) * 0.75f) : (new Color(64, 109, 164) * 0.5f), sb: sb, R: r);
                
                float num2 = num * 0.5f;
                Vector2 vector = r.Right() - Vector2.UnitX * num * (12f + num2 * (float)Knowledge.Texture.Width());
                

                sb.Draw(Knowledge.Texture.Value, vector, null, Color.White * opacity, 0f, new Vector2(0f, Knowledge.Texture.Height() / 2f), num2, (SpriteEffects)0, 0f);
                Utils.DrawBorderString(color: new Color((int)Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor / 5, (int)Main.mouseTextColor) * opacity, sb: sb, text: name, pos: vector - Vector2.UnitX * 10f, scale: num * 0.9f, anchorx: 1f, anchory: 0.4f);
                
                if (num3)
                {
                    OnMouseOver();
                }
            }
        }

        private void OnMouseOver()
        {
            /*if (!PlayerInput.IgnoreMouseInterface)
            {
                Main.player[Main.myPlayer].mouseInterface = true;
                if (Main.mouseLeft && Main.mouseLeftRelease)
                {
                    Main.mouseLeftRelease = false;
                    IngameFancyUI.OpenAchievementsAndGoto(_theAchievement);
                    timeLeft = 0;
                    ShouldBeRemoved = true;
                }
            }*/
        }

        public void DrawInNotificationsArea(SpriteBatch spriteBatch, Rectangle area, ref int gamepadPointLocalIndexTouse)
        {
            Utils.DrawInvBG(spriteBatch, area, Color.Red);
        }
    }
}
