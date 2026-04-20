using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RunesMod.ModUtils;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.UI;
using Terraria.UI;
using Terraria.UI.Chat;

namespace RunesMod.UI.Elements
{
    public class ItemSlotUI : UIElement
    {
        private Item item = new Item();

        public AutoAsset<Texture2D> Texture { get; set; }

        public float Scale { get; set; } = 1f;
        
        public bool CustomBegin { get; set; } = false;

        public DrawingData DrawBeginData { get; set; } = DrawingData.UIDrawing;

        public Item Item
        {
            get { return item; }

            set { item = value ?? new Item(); }
        }

        public Vector2 NormalizedOrigin { get; set; } = Vector2.Zero;

        public bool OnlyDrawing { get; set; } = false;

        public bool CanViewTooltip { get; set; } = true;

        public Action<Item> PostReplaceItems { get; set; } = null;

        public ItemSlotUI(AutoAsset<Texture2D> texture)
        {
            if (texture?.Value == null) return;

            Texture = texture;
            Width.Set(texture.Asset.Width(), 0f);
            Height.Set(texture.Asset.Height(), 0f);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsMouseHovering)
            {
                Main.LocalPlayer.mouseInterface = true;

                if (Item.type > ItemID.None && Item.stack > 0 && CanViewTooltip)
                {
                    Main.hoverItemName = Item.Name;
                    if (Item.stack > 1)
                    {
                        Main.hoverItemName = Main.hoverItemName + " (" + Item.stack + ")";
                    }

                    Main.HoverItem = Item.Clone();
                    Main.HoverItem.tooltipContext = ItemSlot.Context.InventoryItem;
                }
            }

            CalculatedStyle dimensions = GetDimensions();
            Vector2 size = Texture?.Asset?.Size() ?? new(52, 52);
            Vector2 position = dimensions.Position() + size * (1f - Scale) / 2f + size * NormalizedOrigin;
            Vector2 origin = size * NormalizedOrigin;

            position = position.Floor();

            if (CustomBegin)
            {
                spriteBatch.End();
                DrawBeginData.Begin(spriteBatch);
            }

            if (Texture?.Value != null)
            {
                spriteBatch.Draw(Texture.Asset.Value, position, null, Color.White, 0, origin, Scale, SpriteEffects.None, 0f);
            }

            if (Item != null || Item?.active == true || !Item?.IsAir == true) 
            {
                Vector2 itemPosition = position - origin + (new Vector2(52) / 2);

                ItemSlot.DrawItemIcon(Item, ItemSlot.Context.InventoryItem, spriteBatch, itemPosition, 1f, 52 - 24, Color.White);

                if (Item.stack > 1)
                {
                    ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.ItemStack.Value, Item.stack.ToString(), position + new Vector2(10f, 26f) * Scale, Color.White, 0f, Vector2.Zero, new Vector2(Scale), -1f, Scale);
                }
            }

            if (CustomBegin)
            {
                spriteBatch.End();
                spriteBatch.BeginUI();
            }
        }

        public override void LeftClick(UIMouseEvent evt)
        {
            if ((!Item.active && !Main.mouseItem.active) || OnlyDrawing) return;

            if (Main.mouseItem.type == Item.type && ItemLoader.CanStack(Main.mouseItem, Item))
            {
                if (Main.mouseItem.stack + Item.stack <= Item.maxStack)
                {
                    ItemLoader.StackItems(Item, Main.mouseItem, out var _);

                    Main.mouseItem.SetDefaults();

                    if (PostReplaceItems != null)
                        PostReplaceItems(Item);
                }
            }

            else
            {
                Item mouseItem = Main.mouseItem.Clone();
                Main.mouseItem = Item.Clone();
                Item = mouseItem;

                if (PostReplaceItems != null)
                    PostReplaceItems(Item);
            }
            
            SoundEngine.PlaySound(SoundID.Grab);

            base.LeftClick(evt);
        }

        public override void RightClick(UIMouseEvent evt)
        {
            if (!Item.IsAir && !OnlyDrawing)
            {
                if (Main.mouseItem.IsAir)
                {
                    Main.mouseItem = Item.Clone();
                    Main.mouseItem.stack = 1;
                }

                else
                {
                    Main.mouseItem.stack++;
                }

                Item.stack--;

                if (Item.stack <= 0)
                {
                    Item.SetDefaults();
                }
            }
        }
    }
}
