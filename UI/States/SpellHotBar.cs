using Microsoft.Xna.Framework.Input;
using RunesMod.Spells;
using RunesMod.Spells.Elements.Fire;
using RunesMod.Spells.Elements.Earth;
using RunesMod.UI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.UI;
using Microsoft.Xna.Framework;
using Terraria;
using Newtonsoft.Json.Linq;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.ID;

namespace RunesMod.UI.States
{
    public class SpellHotBar : AutoUIState
    {
        public static InterfaceData SpellHotBarInterface => new InterfaceData("Spell Hot Bar", "Mouse Text");

        public override InterfaceData? UserInterface => SpellHotBarInterface; 

        public float Alignment { get; set; } = 0.25f;

        public int? HoldIndex 
        {
            get
            {
                return holdIndex;
            }

            set
            {
                if (value == null)
                {
                    holdIndex = null;
                    return;
                }

                int count = spellSlots.Count;
                holdIndex = holdIndex = ((value % count) + count) % count;
            } 
        }

        private int? holdIndex = null;

        public int timer;
        
        public List<SpellSlotUI> spellSlots = new List<SpellSlotUI>();

        public UIImageButton hideButton;
        public UIImageButton addButton;
        public UIImageButton subButton;

        public override void OnInitialize()
        {
            hideButton = new UIImageButton(GetHotBarTexture("SpellHideButton"));
            hideButton.VAlign = Alignment;
            hideButton.Top.Pixels = -14;
            hideButton.OnLeftClick += new MouseEvent(HideButtonClicked);
            hideButton.OnLeftMouseUp += new MouseEvent(HideButtonUp);
            Append(hideButton);
        }

        public override void Update(GameTime gameTime)
        {
            if (Main.keyState.IsKeyDown(Keys.Right) && timer > 8) Scroll(1);
            if (Main.keyState.IsKeyDown(Keys.Left) && timer > 8) Scroll(-1);

            timer++;
            base.Update(gameTime);
        }

        #region Spell Slots

        public void RecalculateSlots()
        {
            if (spellSlots.Count <= 0) return;
            int offset = 46;

            int lastIndex = spellSlots.Count - 1;
            int x = offset * lastIndex;

            for (int i = lastIndex; i >= 0; i--)
            {
                SpellSlotUI slot = spellSlots[i];

                slot.Frame = 1;
                slot.VAlign = Alignment;
                slot.Left.Pixels = -(6 + 4);
                slot.Left.Pixels += x;
                Append(slot);

                x -= offset;
            }

            if (spellSlots.Count >= 2)
            {
                spellSlots[0].Frame = 0;
                spellSlots[spellSlots.Count - 1].Frame = 2;
            }

            else
            {
                spellSlots[0].Frame = 3;
            }
        }

        public void AddSlot(Spell spell, bool recalculate = true)
        {
            if (spellSlots == null) spellSlots = new();

            SpellSlotUI newSlot = new SpellSlotUI();
            newSlot.Spell = spell;
            spellSlots.Add(newSlot);

            if (recalculate)
                RecalculateSlots();
        }

        public void RemoveSlot() 
        { 
            
        }

        public void ClearSlots()
        {
            foreach (SpellSlotUI slot in spellSlots)
            {
                slot.Remove();
            }

            spellSlots.Clear();
        }

        public Spell GetSpell()
        {
            if (HoldIndex.HasValue)
            {
                return spellSlots[HoldIndex.Value].Spell;
            }

            return null;
        }

        private void Scroll(int direction)
        {
            if (spellSlots.Count > 0)
            {
                if (!HoldIndex.HasValue)
                {
                    HoldIndex = 0;
                }
            }

            else return;

            spellSlots[HoldIndex.Value].IsToggled = false;
            HoldIndex += direction;
            spellSlots[HoldIndex.Value].IsToggled = true;

            timer = 0;
        }

        #endregion

        private void OpenButtonClicked(UIMouseEvent evt, UIElement listeningElement)
        {
            SoundEngine.PlaySound(SoundID.MenuOpen);
        }

        private void HideButtonClicked(UIMouseEvent evt, UIElement listeningElement)
        {
            hideButton.SetImage(GetHotBarTexture("SpellHideButton"));
        }

        private void HideButtonUp(UIMouseEvent evt, UIElement listeningElement)
        {
            SoundEngine.PlaySound(SoundID.MenuClose);
            hideButton.SetImage(GetHotBarTexture("SpellHideButtonClick"));
        }

        private Asset<Texture2D> GetHotBarTexture(string name)
        {
            return ModAssets.RequestAsync<Texture2D>(ModAssets.UITextures, "SpellHotBar/" + name);
        }
    }
}
