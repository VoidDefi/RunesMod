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

namespace RunesMod.UI.States
{
    public class BloodBar : UIState
    {
        public UIText bloodCount;

        public override void OnInitialize()
        {
            bloodCount = new UIText("0", 2);
            bloodCount.HAlign = 0.5f;
            bloodCount.VAlign = 0.5f;
            bloodCount.Top.Pixels = 50f;
            bloodCount.TextColor = Color.Red;
            Append(bloodCount);
        }

        public override void Update(GameTime gameTime)
        {
            bloodCount.SetText(((int)Main.LocalPlayer.SpellSystem().bloodLevel).ToString());

            base.Update(gameTime);
        }
    }

    [Autoload(Side = ModSide.Client)]
    internal class BloodBarSystem : ModSystem
    {
        private static UserInterface Interface { get; set; }

        public static BloodBar BloodBar { get; set; }

        public override void Load()
        {
            BloodBar = new();
            Interface = new();
        }

        public override void UpdateUI(GameTime gameTime)
        {
            if (Main.LocalPlayer.HeldItem.type == ModContent.ItemType<BloodyPin>())
            {
                Show();
            }

            else
            {
                Hide();
            }

            Interface?.Update(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int resourceBarIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Radial Hotbars"));
            if (resourceBarIndex != -1)
            {
                layers.Insert(resourceBarIndex, new LegacyGameInterfaceLayer(
                    "RunesMod: Blood Bar",
                    delegate
                    {
                        Interface.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }

        public static void Hide()
        {
            Interface.SetState(null);
        }

        public static void Show()
        {
            Interface.SetState(BloodBar);
        }
    }
}
