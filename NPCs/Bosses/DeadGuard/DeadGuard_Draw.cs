using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Runtime.CompilerServices;
using Terraria;

namespace RunesMod.NPCs.Bosses.DeadGuard
{
    public partial class DeadGuard
    {
        public override void FindFrame(int frameHeight)
        {
            if (MathF.Abs(NPC.velocity.Y) <= 0.5f)
            {
                float speed = NPC.velocity.Length();
                NPC.frameCounter += (1f / 5f) * (speed < 0.5 ? (NPC.frame.Y == frameHeight ? 0 : 1f) : 1f);
                if (NPC.frameCounter > 1)
                {
                    NPC.frameCounter = 0;
                    NPC.frame.Y += frameHeight;

                    if (NPC.frame.Y >= frameHeight * Main.npcFrameCount[Type])
                    {
                        NPC.frame.Y = frameHeight;
                    }
                }
            }

            else
            {
                NPC.frameCounter = 0;
                NPC.frame.Y = 0;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            return base.PreDraw(spriteBatch, screenPos, drawColor);
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            base.PostDraw(spriteBatch, screenPos, drawColor);
        }

        public override void DrawBehind(int index)
        {
            if (Transferring)
            {
                Main.instance.DrawCacheNPCsMoonMoon.Add(index);
            }
        }
    }
}
