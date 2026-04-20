using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using RunesMod.ModUtils;
using RunesMod.TileEntities;
using RunesMod.Tiles.CraftStations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria;
using RunesMod.UI.Elements;
using SteelSeries.GameSense;
using Terraria.ID;

namespace RunesMod.UI.States
{
    public class SimpleAlchemicalTableState : UIState
    {
        private static string TableTextures => ModAssets.UITextures + "SimpleAlchemicalTable\\";

        private readonly AutoAsset<Texture2D> CrossTexture = new(ModAssets.UITextures, "Cross");
        private readonly AutoAsset<Texture2D> TableTexture = new(TableTextures, "Table");
        private readonly AutoAsset<Texture2D> FlameTexture = new(TableTextures, "Flame");
        private readonly AutoAsset<Texture2D> LiquidTexture = new(TableTextures, "Liquid");
        private readonly AutoAsset<Texture2D> HighlightTexture = new(TableTextures, "Highlight");

        public UIImage table;
        public TextureUI liquid;
        public UIImage highlight;

        public UIImageButton cross;
        public ItemSlotUI alchemySlot;

        public List<ItemSlotUI> usedItems;

        public override void OnInitialize()
        {
            table = new UIImage(TableTexture.Asset);
            table.HAlign = 0.5f;
            table.VAlign = 0.5f;
            table.Left.Pixels = 0;
            table.Top.Pixels = 0;
            Append(table);

            liquid = new TextureUI(LiquidTexture.Asset);
            liquid.HAlign = 0.5f;
            liquid.VAlign = 0.5f;
            liquid.Top.Pixels = -81;
            liquid.Left.Pixels = -1;
            liquid.CustomBegin = true;
            liquid.DrawBeginData = DrawingData.UIDrawing;

            Effect gradient = ModAssets.Request<Effect>(ModAssets.Effects, "ColorGradient").Value;
            gradient.Parameters["uHighlightColor"].SetValue(new Color(174, 199, 255).ToVector3());
            gradient.Parameters["uShadowColor"].SetValue(new Color(0, 0, 81).ToVector3());

            liquid.DrawBeginData.Effect = gradient;
            liquid.DrawBeginData.BlendState = BlendState.AlphaBlend;
            Append(liquid);

            highlight = new UIImage(HighlightTexture.Asset);
            highlight.HAlign = 0.5f;
            highlight.VAlign = 0.5f;
            highlight.Left.Pixels = -25;
            highlight.Top.Pixels = -95;
            Append(highlight);

            cross = new UIImageButton(CrossTexture.Asset);
            cross.VAlign = 0.5f;
            cross.HAlign = 0.5f;
            cross.Top.Pixels = -170;
            cross.Left.Pixels = 170;
            cross.OnLeftClick += new MouseEvent(CloseMenu);
            Append(cross);

            alchemySlot = new ItemSlotUI(new(ModAssets.UITextures, "AlchemySlot"));
            alchemySlot.VAlign = 0.5f;
            alchemySlot.HAlign = 0.5f;
            alchemySlot.PostReplaceItems = (slot) => ThrowItemInBottle();
            Append(alchemySlot);

            usedItems = new List<ItemSlotUI>();
        }

        private void ThrowItemInBottle()
        {
            AddUsedItem(alchemySlot.Item);
            alchemySlot.Item.SetDefaults();
        }

        private void AddUsedItem(Item item)
        {
            foreach (ItemSlotUI slot in usedItems)
                slot.Remove();

            ItemSlotUI slotUI = new ItemSlotUI(null);
            slotUI.Item = item.Clone();

            usedItems.Add(slotUI);

            for (int i = 0; i < usedItems.Count; i++)
            {
                ItemSlotUI slot = usedItems[i];

                slot.VAlign = 0.5f;
                slot.HAlign = 0.5f;

                slot.Top.Pixels = -180;
                slot.Left.Pixels = ((float)i - (float)(usedItems.Count - 1) / 2f) * 20;

                slot.Width.Set(52, 0f);
                slot.Height.Set(52, 0f);

                slot.OnlyDrawing = true;
                slot.CanViewTooltip = false;

                Append(slot);
            }
        }

        public void ClearUsedItems()
        {
            foreach (ItemSlotUI slot in usedItems)
                slot.Remove();

            usedItems.Clear();
        }

        private void CloseMenu(UIMouseEvent evt, UIElement listeningElement)
        {
            liquid.DrawBeginData.Effect.Parameters["uHighlightColor"].SetValue(new Color(174, 199, 255).ToVector3() * 1.1f);
            liquid.DrawBeginData.Effect.Parameters["uShadowColor"].SetValue(new Color(0, 0, 81).ToVector3() * 1.1f);

            highlight.Left.Pixels = -25;
            highlight.Top.Pixels = -95;

            SimpleAlchemicalTableSystem.Hide();
        }
    }

    [Autoload(Side = ModSide.Client)]
    internal class SimpleAlchemicalTableSystem : ModSystem
    {
        public static SimpleAlchemicalTableEntity Entity { get; set; }

        private static UserInterface Interface { get; set; }

        public static SimpleAlchemicalTableState BasicTable { get; set; }

        public override void Load()
        {
            BasicTable = new();
            Interface = new();
        }

        public override void UpdateUI(GameTime gameTime)
        {
            if (Interface?.CurrentState != null)
            {
                if (!Main.LocalPlayer.adjTile[ModContent.TileType<SimpleAlchemicalTable>()])
                {
                    Hide();
                }
            }

            Interface?.Update(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int resourceBarIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (resourceBarIndex != -1)
            {
                layers.Insert(resourceBarIndex, new LegacyGameInterfaceLayer(
                    "RunesMod: BaseTable",
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
            Entity = null;
            Interface.SetState(null);
        }

        public static void Show(SimpleAlchemicalTableEntity entity)
        {
            Entity = entity;
            Interface.SetState(BasicTable);
        }
    }
}
