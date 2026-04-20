using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace RunesMod.ModUtils
{
    public struct DrawingData
    {
        public static DrawingData UIDrawing => new(matrix: () => Main.UIScaleMatrix);

        public static DrawingData UIPointDrawing => new(samplerState: SamplerState.PointClamp, matrix: () => Main.UIScaleMatrix);

        public static DrawingData WorldDrawing => new(samplerState: SamplerState.PointClamp, matrix: () => Main.GameViewMatrix.TransformationMatrix);



        public SpriteSortMode SortMode { get; set; } = SpriteSortMode.Deferred;

        public BlendState BlendState { get; set; } = BlendState.AlphaBlend;

        public SamplerState SamplerState { get; set; } = SamplerState.LinearClamp;

        public DepthStencilState DepthStencilState { get; set; } = DepthStencilState.None;

        public RasterizerState RasterizerState { get; set; } = RasterizerState.CullCounterClockwise;
        
        public Effect Effect { get; set; } = null;

        public Func<Matrix?> Matrix { get; set; } = null;

        public Matrix TransformMatrix => Matrix?.Invoke() ?? Microsoft.Xna.Framework.Matrix.Identity;

        public DrawingData(SpriteSortMode? sortMode = null, BlendState blendState = null, SamplerState samplerState = null, DepthStencilState depthStencilState = null, RasterizerState rasterizerState = null, Effect effect = null, Func<Matrix?> matrix = null)
        {
            SortMode = sortMode ?? SpriteSortMode.Deferred;
            BlendState = blendState ?? BlendState.AlphaBlend;
            SamplerState = samplerState ?? SamplerState.LinearClamp;
            DepthStencilState = depthStencilState ?? DepthStencilState.None;
            RasterizerState = rasterizerState ?? RasterizerState.CullCounterClockwise;
            Effect = effect;
            Matrix = matrix;
        }

        public void Begin(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin
            (
                SortMode,
                BlendState,
                SamplerState,
                DepthStencilState,
                RasterizerState,
                Effect,
                TransformMatrix
            );
        }
    }
}
