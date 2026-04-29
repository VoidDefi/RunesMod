using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using RunesMod.ModUtils;
using RunesMod.UI.Elements;
using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria;
using Terraria.UI;
using System;
using System.Linq;
using Terraria.GameContent.UI.Elements;
using RunesMod.Items.SlotProtectors;
using RunesMod.Cooldowns;

namespace RunesMod.UI.States
{
    public class CooldownUI : UIState
    {
        public UIElement point;

        public override void OnInitialize()
        {
            point = new UIElement();
            point.HAlign = 0.0f;
            point.VAlign = 0.5f;
            point.Top.Pixels = 0f;
            point.Left.Pixels = 100f;
            Append(point);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 position = point.GetDimensions().Position();

            List<Cooldown> cooldowns = Main.LocalPlayer.CooldownSystem().Cooldowns;

            if (cooldowns != null)
            {
                Vector2 offset = new(0, 0);

                foreach (Cooldown cooldown in cooldowns)
                {
                    cooldown.Draw(spriteBatch, position + offset);
                    offset.X += 32 + 10;
                }
            }
        }
    }

    [Autoload(Side = ModSide.Client)]
    internal class CooldownUISystem : ModSystem
    {
        private static UserInterface Interface { get; set; }

        public static CooldownUI CooldownUI { get; set; }

        public override void Load()
        {
            CooldownUI = new();
            Interface = new();
            Interface.SetState(CooldownUI);
        }

        public override void UpdateUI(GameTime gameTime)
        {
            Interface?.Update(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int resourceBarIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Radial Hotbars"));
            if (resourceBarIndex != -1)
            {
                layers.Insert(resourceBarIndex, new LegacyGameInterfaceLayer(
                    "RunesMod: Cooldown UI",
                    delegate
                    {
                        Interface.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }
    }
}
