using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RunesMod.ModUtils;

namespace RunesMod.Graphics
{
    public class UnitedDustDrawing : ModSystem
    {
        private struct DustData
        {
            public UnitedDust definition;
            public RenderTarget2D target;
            public bool[] dusts;

            public DustData(UnitedDust definition, RenderTarget2D target)
            {
                this.definition = definition;
                this.target = target;

                dusts = new bool[Main.maxDust];
            }
        }

        private static int MinDust => 200;

        private static Dictionary<int, DustData> dusts = new();

        public override void Load()
        {
            On_Main.Draw_Inner += OnDrawInnerHook;
            On_Main.DrawProjectiles += OnDrawProjectilesHook;
        }

        private void OnDrawInnerHook(On_Main.orig_Draw_Inner original, Main self, GameTime gameTime)
        {
            if (!Main.gameMenu)
            {
                GraphicsDevice device = Main.graphics.GraphicsDevice;

                foreach (var dustData in dusts)
                {
                    DustData data = dustData.Value;

                    device.SetRenderTarget(data.target);

                    //Begin Drawing Dusts

                    device.Clear(Color.Transparent);

                    //DrawingData - SpriteBatch data buffer
                    DrawingData? dustDrawingData = data.definition.GetDustDrawingData();

                    if (dustDrawingData.HasValue)
                    {
                        dustDrawingData.Value.Begin(Main.spriteBatch);
                    }

                    else Main.spriteBatch.Begin(null, Main.Transform);

                    //Drawing Dusts in Target

                    for (int i = 0; i < data.dusts.Length; i++)
                    {
                        if (data.dusts[i])
                        {
                            Dust dust = Main.dust[i];

                            data.definition.Draw(Main.spriteBatch, dust);
                        }

                        //clear current dust
                        data.dusts[i] = false;
                    }

                    //End Drawing Dusts
                    Main.spriteBatch.End();
                }

                device.SetRenderTarget(null);
            }

            original.Invoke(self, gameTime);
        }

        private void OnDrawProjectilesHook(On_Main.orig_DrawProjectiles original, Main self)
        {
            original.Invoke(self);

            //Draw Target

            foreach (var dustData in dusts)
            {
                DustData data = dustData.Value;

                DrawingData? canvasDrawingData = data.definition.GetCanvasDrawingData();

                if (canvasDrawingData.HasValue)
                {
                    canvasDrawingData.Value.Begin(Main.spriteBatch);
                }
                else Main.spriteBatch.Begin();

                data.definition.DrawCanvas(Main.spriteBatch, data.target);

                Main.spriteBatch.End();
            }
        }

        public static void AddToPipeline(UnitedDust definition, Dust dust)
        {
            if (definition == null || dust == null) return;
            if (definition.Type != dust.type) return;

            if (dusts.TryGetValue(definition.Type, out DustData data))
            {
                if (dust.dustIndex < data.dusts.Length && dust.dustIndex >= 0)
                {
                    data.dusts[dust.dustIndex] = true;
                }
            }

            else
            {
                RenderTarget2D target = null;
                RenderTarget2D customTarget = definition.CreateRenderTarget();

                if (customTarget == null)
                {
                    GraphicsDevice device = Main.graphics.GraphicsDevice;
                    target = new RenderTarget2D(device, Main.screenWidth, Main.screenHeight, false, device.PresentationParameters.BackBufferFormat, (DepthFormat)0);
                }

                else target = customTarget;

                dusts.Add(definition.Type, new DustData(definition, target));
            }
        }
    }
}
