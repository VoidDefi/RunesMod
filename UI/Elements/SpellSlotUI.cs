using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Terraria.UI;
using Microsoft.Xna.Framework;
using RunesMod.Spells;
using ReLogic.Content;

namespace RunesMod.UI.Elements
{
    public class SpellSlotUI : UIElement
    {
        public static float PullOutSpeed => 0.005f;

        public static float BacklightingSpeed => 0.2f;

        public static Color DeactivateColor => Color.Gray;

        public Asset<Texture2D> SlotTexture { get; set; } = null;

        public Spell Spell { get; set; } = null;

        public int Frame { get; set; } = 1;

        public bool IsToggled { get; set; } = false;

        public Color Color { get; set; } = Color.White;

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Main.playerInventory) return;

            if (SlotTexture == null)
                SlotTexture = ModAssets.Request<Texture2D>(ModAssets.UITextures, "SpellHotBar/SpellSlot");

            CalculatedStyle style = GetInnerDimensions();
            Vector2 position = style.Position();

            Rectangle slotFrame = SlotTexture.Frame(4, frameX: Frame);
            slotFrame.Width--;

            if (IsToggled)
            {
                position.X = MathHelper.Lerp(position.X + 2, position.X, PullOutSpeed);
                Color = Color.Lerp(Color, Color.White, BacklightingSpeed);
            }

            else
            {
                position.X = MathHelper.Lerp(position.X, position.X + 2, PullOutSpeed);
                Color = Color.Lerp(Color, DeactivateColor, BacklightingSpeed);
            }

            Vector2 spellOffset = new Vector2(12 + 4, 4);

            spriteBatch.Draw(SlotTexture.Value, position, slotFrame, Color, 0f, Vector2.Zero, 1f, 0, 1f);
            spriteBatch.Draw(Spell.Texture.Value, position + spellOffset, null, Color, 0f, Vector2.Zero, 1f, 0, 1f);
        }
    }
}
