using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;

namespace RunesMod.NPCs.Bosses.DeadGuard
{
    public partial class DeadGuardHand
    {
        public float DrawOffsetX
        {
            get => NPC.localAI[0];
            set => NPC.localAI[0] = value;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D bone = ModAssets.Request<Texture2D>(ModAssets.BossTextures, "DeadGuardBone").Value;

            Vector2 offset = new Vector2(NPC.width * 0.5f - 2f * HandBaseDirection, NPC.height);
            //offset = offset.RotatedBy(NPC.rotation * NPC.spriteDirection + MathF.PI, NPC.Size);

            Vector2 vector5 = NPC.position + offset;
            //vector5 = vector5.RotatedBy(NPC.rotation);
            for (int j = 0; j < 2; j++)
            {
                float num6 = Body.Center.X - vector5.X;
                float num7 = Body.Center.Y - vector5.Y;
                float num8 = 0f;
                if (j == 0)
                {
                    num6 += 200f * HandBaseDirection;
                    num7 -= 200f;
                    num8 = (float)Math.Sqrt(num6 * num6 + num7 * num7);
                    num8 = 50f / num8;
                    vector5.X += num6 * num8;
                    vector5.Y += num7 * num8;
                }
                else
                {
                    num6 += 50f * HandBaseDirection;
                    num7 += 10f;
                    num8 = (float)Math.Sqrt(num6 * num6 + num7 * num7);
                    num8 = (25f) / num8;
                    vector5.X += num6 * num8;
                    vector5.Y += num7 * num8;
                }
                float rotation5 = (float)Math.Atan2(num7, num6);
                Color color5 = Lighting.GetColor((int)vector5.X / 16, (int)(vector5.Y / 16f));
                Main.spriteBatch.Draw(bone, new Vector2(vector5.X - screenPos.X, vector5.Y - screenPos.Y), (Rectangle?)new Rectangle(0, 0, bone.Width, bone.Height), color5, rotation5, new Vector2(bone.Width * 0.5f, bone.Height * 0.5f), 1f, 0, 0f);
                if (j == 0)
                {
                    vector5.X += num6 * num8 / 2f;
                    vector5.Y += num7 * num8 / 2f;
                }
            }

            Texture2D hand = ModContent.Request<Texture2D>(Texture, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            Vector2 position = NPC.position - screenPos;// + offset;
            position.Y += offset.Y;
            position.X += offset.X * 2;

            Main.spriteBatch.Draw(hand, position, NPC.frame, drawColor, NPC.rotation * 0, NPC.Size, 1f, NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : 0, 0f);

            return false;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            base.PostDraw(spriteBatch, screenPos, drawColor);
        }

        public override void FindFrame(int frameHeight)
        {
            if (HandType == HandTypes.Axe)
            {
                NPC.frame.Y = frameHeight;
            }
        }

        public override void DrawBehind(int index)
        {
            if (BodyModNPC.Transferring)
            {
                Main.instance.DrawCacheNPCsMoonMoon.Add(index);
            }
        }
    }
}
