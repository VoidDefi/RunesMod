using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json.Linq;
using ReLogic.Content;
using RunesMod.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.UI;

namespace RunesMod.Cooldowns
{
    public abstract class Cooldown : ModType
    {
        public short Type { get; internal set; }

        public Asset<Texture2D> Texture { get; set; }

        private float maxValue = 0f;

        public float MaxValue
        {
            get => maxValue;
            set => maxValue = Math.Clamp(value, 0, float.MaxValue);
        }

        private float curValue = 0f;

        public float Value
        {
            get => Math.Clamp(curValue, -1, MaxValue); //For set Value before MaxValue
            set => curValue = value;
        }

        protected sealed override void Register()
        {
            ModTypeLookup<Cooldown>.Register(this);
            Type = (short)CooldownLoader.ReserveCooldownID();

            CooldownLoader.Add(this);

            Load();
        }

        public virtual void Decrement(Player player)
        {
            Value--;
        }

        public virtual void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            if (Texture?.Value != null)
            {
                spriteBatch.Draw(Texture.Value, position, null, Color.White, 0f, Texture.Size() / 2f, Texture.Size() / 32, 0, 0);
                DrawBar(spriteBatch, position + new Vector2(0, 32 - 8), 1f);
            }
        }

        public virtual void End(Player player)
        {

        }

        //From Main.DrawHealthBar
        protected void DrawBar(SpriteBatch spriteBatch, Vector2 position, float alpha, float scale = 1f, bool noFlip = false)
        {
            if (Value <= 0)
            {
                return;
            }
            float num = (float)Value / (float)MaxValue;
            if (num > 1f)
            {
                num = 1f;
            }
            int num2 = (int)(36f * num);
            float num3 = position.X - 18f * scale;
            float num4 = position.Y;
            /*if (player[myPlayer].gravDir == -1f && !noFlip)
            {
                num4 -= screenPosition.Y;
                num4 = screenPosition.Y + (float)screenHeight - num4;
            }*/
            float num5 = 0f;
            float num6 = 0f;
            float num7 = 0f;
            float num8 = 255f;
            num -= 0.1f;
            if ((double)num > 0.5)
            {
                num6 = 255f;
                num5 = 255f * (1f - num) * 2f;
            }
            else
            {
                num6 = 255f * num * 2f;
                num5 = 255f;
            }
            float num9 = 0.95f;
            num5 = num5 * alpha * num9;
            num6 = num6 * alpha * num9;
            num8 = num8 * alpha * num9;
            if (num5 < 0f)
            {
                num5 = 0f;
            }
            if (num5 > 255f)
            {
                num5 = 255f;
            }
            if (num6 < 0f)
            {
                num6 = 0f;
            }
            if (num6 > 255f)
            {
                num6 = 255f;
            }
            if (num8 < 0f)
            {
                num8 = 0f;
            }
            if (num8 > 255f)
            {
                num8 = 255f;
            }
            Color color = new Color((byte)num5, (byte)num6, (byte)num7, (byte)num8);
            if (num2 < 3)
            {
                num2 = 3;
            }
            if (num2 < 34)
            {
                if (num2 < 36)
                {
                    spriteBatch.Draw(TextureAssets.Hb2.Value, new Vector2(num3/* - screenPosition.X*/ + num2 * scale, num4/* - screenPosition.Y*/), (Rectangle?)new Rectangle(2, 0, 2, TextureAssets.Hb2.Height()), color, 0f, new Vector2(0f, 0f), scale, 0, 0f);
                }
                if (num2 < 34)
                {
                    spriteBatch.Draw(TextureAssets.Hb2.Value, new Vector2(num3/* - screenPosition.X*/ + (num2 + 2) * scale, num4/* - screenPosition.Y*/), (Rectangle?)new Rectangle(num2 + 2, 0, 36 - num2 - 2, TextureAssets.Hb2.Height()), color, 0f, new Vector2(0f, 0f), scale, 0, 0f);
                }
                if (num2 > 2)
                {
                    spriteBatch.Draw(TextureAssets.Hb1.Value, new Vector2(num3/* - screenPosition.X*/, num4/* - screenPosition.Y*/), (Rectangle?)new Rectangle(0, 0, num2 - 2, TextureAssets.Hb1.Height()), color, 0f, new Vector2(0f, 0f), scale, 0, 0f);
                }
                spriteBatch.Draw(TextureAssets.Hb1.Value, new Vector2(num3/* - screenPosition.X*/ + (num2 - 2) * scale, num4/* - screenPosition.Y*/), (Rectangle?)new Rectangle(32, 0, 2, TextureAssets.Hb1.Height()), color, 0f, new Vector2(0f, 0f), scale, 0, 0f);
            }
            else
            {
                if (num2 < 36)
                {
                    spriteBatch.Draw(TextureAssets.Hb2.Value, new Vector2(num3/* - screenPosition.X*/ + num2 * scale, num4/* - screenPosition.Y*/), (Rectangle?)new Rectangle(num2, 0, 36 - num2, TextureAssets.Hb2.Height()), color, 0f, new Vector2(0f, 0f), scale, 0, 0f);
                }
                spriteBatch.Draw(TextureAssets.Hb1.Value, new Vector2(num3/* - screenPosition.X*/, num4/* - screenPosition.Y*/), (Rectangle?)new Rectangle(0, 0, num2, TextureAssets.Hb1.Height()), color, 0f, new Vector2(0f, 0f), scale, 0, 0f);
            }
        }
    }
}
