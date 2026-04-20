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

namespace RunesMod.UI.States
{
    public class KnowledgesButton : UIState
    {
        private readonly AutoAsset<Texture2D> BookTexture = new(ModAssets.KnowledgesUI, "BookButton");
        private readonly AutoAsset<Texture2D> BookMarkTexture = new(ModAssets.KnowledgesUI, "ButtonBookMark");

        private readonly AutoAsset<Texture2D> GlareTexture = new(ModAssets.MaskTextures, "Glare");
        private readonly AutoAsset<Effect> OverlappingEffect = new(ModAssets.Effects, "Overlapping");

        private int BookMarkFrames => 5;

        private int BookFrames => 3;

        public TextureUI book;
        public TextureUI bookMark;

        public bool Closing;

        public float bookMarkFrame = 0;
        public float bookFrame = 0;

        public bool isHover = false;

        public float angle = 0;

        public bool drawGlare = true;

        public override void OnInitialize()
        {
            book?.Remove();
            bookMark?.Remove();

            book = new TextureUI(BookTexture.Asset);
            book.HAlign = 1f;
            book.VAlign = 0f;
            book.Top.Pixels = 100f;
            book.Left.Pixels = -314f;
            book.OnMouseOver += OnHover;
            book.OnMouseOut += OnOut;
            book.OnLeftClick += OnClick;
            book.Frame = BookMarkTexture.Asset.Frame(1, BookFrames);
            Append(book);

            bookMark = new TextureUI(BookMarkTexture.Asset);
            bookMark.HAlign = 1f;
            bookMark.VAlign = 0f;
            bookMark.Top.Pixels = 40f;
            bookMark.Left.Pixels = -14f;
            bookMark.Frame = BookMarkTexture.Asset.Frame(1, BookMarkFrames);
            book.Append(bookMark);
        }

        private void OnOut(UIMouseEvent evt, UIElement listeningElement)
        {
            isHover = false;
        }

        private void OnHover(UIMouseEvent evt, UIElement listeningElement)
        {
            isHover = true;
            //drawGlare = false;
        }

        private void OnClick(UIMouseEvent evt, UIElement listeningElement)
        {
            drawGlare = true;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //Animation

            if (isHover)
            {
                if (bookFrame < BookFrames - 1) bookFrame += 15f / 60f;
                if (bookFrame > BookFrames - 1) bookFrame = BookFrames - 1;
            }

            else
            {
                if (bookFrame > 0) bookFrame -= 15f / 60f;
                if (bookFrame < 0) bookFrame = 0;
            }

            book.Frame = BookTexture.Asset.Frame(1, BookFrames, frameY: (int)bookFrame);

            bookMark.Frame = BookMarkTexture.Asset.Frame(1, BookMarkFrames, frameY: (int)bookMarkFrame);
            bookMarkFrame += 6f / 60f;

            if (bookMarkFrame >= BookMarkFrames) bookMarkFrame = 0;

            //Drawing

            if (drawGlare)
            {
                DrawingData glareBegin = DrawingData.UIDrawing;
                glareBegin.Effect = OverlappingEffect.Value;
                glareBegin.Effect.Parameters["uColor"].SetValue(new Vector4(3f, 0f, 0, 0.1f) * 0.5f);
                glareBegin.Effect.Parameters["uHotColor"].SetValue(new Vector4(1.5f, 1f, 1f, 0.1f) * 0.5f);
                glareBegin.Effect.Parameters["uHotFactor"].SetValue(1f);

                spriteBatch.End();
                glareBegin.Begin(spriteBatch);

                spriteBatch.Draw(GlareTexture.Value, book.GetDimensions().Center(), null, Color.White, angle, GlareTexture.Value.Size() / 2f, 1.5f, 0, 0);
            }

            spriteBatch.End();
            DrawingData.UIPointDrawing.Begin(spriteBatch);

            base.Draw(spriteBatch);

            spriteBatch.End();
            DrawingData.UIPointDrawing.Begin(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            angle += MathF.PI / (360f);

            base.Update(gameTime);
        }
    }

    [Autoload(Side = ModSide.Client)]
    internal class KnowledgesButtonSystem : ModSystem
    {
        private static UserInterface Interface { get; set; }

        public static KnowledgesButton Button { get; set; }

        public override void Load()
        {
            Button = new();
            Interface = new();
        }

        public override void UpdateUI(GameTime gameTime)
        {

            if (!Main.playerInventory)
            {
                if (Interface?.CurrentState != null)
                {
                    Hide();
                }
            }

            else
            {
                Show();
            }

            Interface?.Update(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int resourceBarIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Radial Hotbars"));
            if (resourceBarIndex != -1)
            {
                layers.Insert(resourceBarIndex, new LegacyGameInterfaceLayer(
                    "RunesMod: Knowledges Button",
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
            Interface.SetState(Button);
        }
    }
}
