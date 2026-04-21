using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RunesMod.ModUtils;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace RunesMod.Graphics
{
    public abstract class UnitedDust : ModDust
    {
        public override string Texture => "RunesMod/Dusts/InvisibleDust";

        public sealed override bool Update(Dust dust)
        {
            if (dust.dustIndex < Main.maxDustToDraw)
            {
                UnitedDustDrawing.AddToPipeline(this, dust);
            }

            return base.Update(dust);
        }

        public sealed override bool PreDraw(Dust dust)
        {
            UnitedDustDrawing.AddToPipeline(this, dust);

            return false;
        }

        public virtual void DrawCanvas(SpriteBatch spriteBatch, RenderTarget2D target)
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch, Dust dust)
        {

        }

        public virtual RenderTarget2D CreateRenderTarget()
        {
            return null;
        }

        public virtual void PostDrawDusts(SpriteBatch spriteBatch, RenderTarget2D target)
        {

        }

        public virtual DrawingData? GetDustDrawingData()
        {
            return null;
        }

        public virtual DrawingData? GetCanvasDrawingData()
        {
            return null;
        }
    }
}