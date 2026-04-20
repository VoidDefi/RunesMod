using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using RunesMod.ModUtils;
using RunesMod.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.UI;
using static Terraria.GameContent.Animations.IL_Actions.Sprites;

namespace RunesMod.UI.Elements
{
    public class ConcentrationBarUI : UIElement
    {
        #region Assets

        public AutoAsset<Texture2D> Frame = new AutoAsset<Texture2D>(ModAssets.ConcentrationUITextures, "ConcentrationFrame");
        
        public AutoAsset<Texture2D> FrameOpen = new AutoAsset<Texture2D>(ModAssets.ConcentrationUITextures, "ConcentrationFrameOpen");
        
        public AutoAsset<Texture2D> Bar = new AutoAsset<Texture2D>(ModAssets.ConcentrationUITextures, "ConcentrationBar");
        
        public AutoAsset<Texture2D> BarBack = new AutoAsset<Texture2D>(ModAssets.ConcentrationUITextures, "ConcentrationBarBack");
        
        public AutoAsset<Texture2D> Bracket = new AutoAsset<Texture2D>(ModAssets.ConcentrationUITextures, "ConcentrationBracket");
        
        public AutoAsset<Texture2D> BracketCenter = new AutoAsset<Texture2D>(ModAssets.ConcentrationUITextures, "ConcentrationBracketCenter");

        #endregion

        public override void Draw(SpriteBatch spriteBatch)
        {
            VAlign = 0f;
            HAlign = 0.5f;
            Recalculate();

            CalculatedStyle style = GetInnerDimensions();
            Vector2 position = style.Position();

            Player player = Main.player[Main.myPlayer];
            Concentration concentration = player.SpellSystem().concentration;

            if (concentration.MaxConcentration <= 0) return;

            spriteBatch.End();
            spriteBatch.BeginUI(SamplerState.PointClamp);

            //Draw Backs
            for (int i = 0; i < concentration.MaxConcentration; i++)
                spriteBatch.Draw(BarBack.Value, position + new Vector2(6 + (i * 24), 4), Color.White, new Vector2(11f, 1f));

            float curLength = 0;
            bool isHalf = false;

            //Draw Bars
            for (int i = 0; i < concentration.SlotCount; i++)
            {
                ConcentrationSlot slot = concentration.GetSlot(i);

                Vector2 barPos = position + new Vector2((isHalf ? 5 : 6) + (curLength * 24), 4);
                Vector2 size = new Vector2(11f * slot.SlotLength + MathF.Floor(slot.SlotLength), 1f);

                ConcentrationBarGradient? gradient = slot.Spell?.concentrationBarGradient;

                if (!gradient.HasValue)
                    gradient = new(Color.White, Color.Gray, Color.DarkGray, Color.DimGray);

                for (int j = 0; j < 3; j++)
                {
                    Rectangle barFrame = Bar.Value.SafeFrame(4, frameX: j);

                    spriteBatch.Draw(Bar.Value, barPos, barFrame, gradient.Value[j], size);
                }

                if (/*(i - 1 >= 0 && */!float.IsInteger(curLength)/*)*/)
                {
                    Rectangle barFrame = Bar.Value.SafeFrame(4, frameX: 3);
                    Color color = concentration.GetSlot(i - 1).Spell.concentrationBarGradient.Value[3];
                    spriteBatch.Draw(Bar.Value, barPos + new Vector2(-1, 0), barFrame, color, 1f);
                }

                Vector2 bracketPos = barPos + new Vector2(2, -14);
                Vector2 bracketSize = size + new Vector2(-4, 0);

                Vector2 openOffset = Vector2.Zero;
                Vector2 closeOffset = Vector2.Zero;

                Vector2 centerSizeOffset = Vector2.Zero;

                if (slot.SlotLength == 0.5f)
                {
                    if (float.IsInteger(curLength))
                    {
                        openOffset = new(-4, 0);
                        closeOffset = new(3, 0);
                    }

                    else
                    {
                        openOffset = new(0, 0);
                        closeOffset = new(7, 0);
                        centerSizeOffset.X = 3f;
                    }
                }

                for (int j = 0; j < 4; j++)
                {
                    Rectangle centerFrame = BracketCenter.Value.SafeFrame(4, frameX: j);
                    spriteBatch.Draw(BracketCenter.Value, bracketPos, centerFrame, gradient.Value[j], bracketSize + centerSizeOffset);
                }

                Vector2 bracketClosePos = bracketPos + (new Vector2(bracketSize.X - 2, 0) * 2f);

                for (int j = 0; j < 4; j++)
                {
                    Rectangle bracketFrame = Bracket.Value.SafeFrame(4, frameX: j);

                    spriteBatch.Draw(Bracket.Value, bracketPos + openOffset, bracketFrame, gradient.Value[j]);
                    spriteBatch.Draw(Bracket.Value, bracketClosePos + closeOffset, bracketFrame, gradient.Value[j], 1f, SpriteEffects.FlipHorizontally);
                }

                Vector2 iconOffset = new Vector2(0, -18);

                if (slot.SlotLength == 0.5)
                {
                    if (float.IsInteger(curLength)) iconOffset = new Vector2(-8, -18);
                    else iconOffset = new Vector2(-3, -18);
                }

                if (slot.SlotLength > 1)
                {
                    iconOffset = new Vector2(bracketSize.X / 2f + 2f, -18);
                }

                spriteBatch.Draw(slot.Spell.Texture.Value, bracketPos + iconOffset, Color.White, 0.5f);

                curLength += slot.SlotLength;
                isHalf = slot.SlotLength == 0.5f; 
            }

            //Draw Frames

            spriteBatch.Draw(FrameOpen.Value, position, Color.White);

            for (int i = 0; i < concentration.MaxConcentration; i++)
                spriteBatch.Draw(Frame.Value, position + new Vector2(6 + (i * 24), 0), Color.White);

            spriteBatch.End();
            spriteBatch.BeginUI();
        }
    }
}
