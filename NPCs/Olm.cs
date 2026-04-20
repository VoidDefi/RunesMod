using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Terraria.Audio;
using RunesMod.Items.Consumable.Catches;

namespace RunesMod.NPCs
{
    public class Olm : ModNPC
	{
		public override void SetStaticDefaults()
		{
            Main.npcFrameCount[Type] = 8;
            Main.npcCatchable[Type] = true;

            NPCID.Sets.CountsAsCritter[Type] = true;
            NPCID.Sets.TakesDamageFromHostilesWithoutBeingFriendly[Type] = true;
            NPCID.Sets.TownCritter[Type] = true;
        }

		public override void SetDefaults()
		{
            NPC.noGravity = true;
            NPC.width = 30;
            NPC.height = 12;
            NPC.aiStyle = 16;
            NPC.damage = 0;
            NPC.defense = 0;
            NPC.lifeMax = 5;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0.5f;
            AIType = NPCID.Goldfish;
            NPC.catchItem = ModContent.ItemType<OlmItem>();
        }

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return SpawnCondition.Cavern.Chance * 0.6f * (spawnInfo.Water ? 1 : 0);
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[2]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns,
				new FlavorTextBestiaryInfoElement("Иногда жизненная энергия мира образует одухотварённые згустки, которые нападают на всё что двигается и высасуют жизнь при касании, если у них нехватает её")
			});
		}

        public override void AI()
		{
            /*Vector2 vector5 = new Vector2(Main.rCurrentNPC.position.X + (float)Main.rCurrentNPC.width * 0.5f - 5f * Main.rCurrentNPC.ai[0], Main.rCurrentNPC.position.Y + 20f);
            for (int j = 0; j < 2; j++)
            {
                float num6 = Main.npc[(int)rCurrentNPC.ai[1]].position.X + (float)(Main.npc[(int)Main.rCurrentNPC.ai[1]].width / 2) - vector5.X;
                float num7 = Main.npc[(int)rCurrentNPC.ai[1]].position.Y + (float)(Main.npc[(int)Main.rCurrentNPC.ai[1]].height / 2) - vector5.Y;
                float num8 = 0f;
                if (j == 0)
                {
                    num6 -= 200f * Main.rCurrentNPC.ai[0];
                    num7 += 130f;
                    num8 = (float)Math.Sqrt(num6 * num6 + num7 * num7);
                    num8 = 92f / num8;
                    vector5.X += num6 * num8;
                    vector5.Y += num7 * num8;
                }
                else
                {
                    num6 -= 50f * Main.rCurrentNPC.ai[0];
                    num7 += 80f;
                    num8 = (float)Math.Sqrt(num6 * num6 + num7 * num7);
                    num8 = 60f / num8;
                    vector5.X += num6 * num8;
                    vector5.Y += num7 * num8;
                }
                float rotation5 = (float)Math.Atan2(num7, num6) - 1.57f;
                Color color5 = Lighting.GetColor((int)vector5.X / 16, (int)(vector5.Y / 16f));
                Main.spriteBatch.Draw(TextureAssets.BoneArm.Value, new Vector2(vector5.X - Main.screenPosition.X, vector5.Y - Main.screenPosition.Y), (Rectangle?)new Rectangle(0, 0, TextureAssets.BoneArm.Width(), TextureAssets.BoneArm.Height()), color5, rotation5, new Vector2((float)TextureAssets.BoneArm.Width() * 0.5f, (float)TextureAssets.BoneArm.Height() * 0.5f), 1f, (SpriteEffects)0, 0f);
                if (j == 0)
                {
                    vector5.X += num6 * num8 / 2f;
                    vector5.Y += num7 * num8 / 2f;
                }
            }*/

            /*
            if (NPC.type == 615)
            {
                if (NPC.ai[2] == 0f)
                {
                    int num787 = Main.rand.Next(300, 1200);
                    if ((NPC.ai[3] += 1f) >= (float)num787)
                    {
                        NPC.ai[2] = Main.rand.Next(1, 3);
                        if (NPC.ai[2] == 1f && !Collision.CanHitLine(NPC.position, NPC.width, NPC.height, new Vector2(NPC.position.X, NPC.position.Y - 128f), NPC.width, NPC.height))
                        {
                            NPC.ai[2] = 2f;
                        }
                        if (NPC.ai[2] == 2f)
                        {
                            NPC.TargetClosest();
                        }
                        NPC.ai[3] = 0f;
                        NPC.netUpdate = true;
                    }
                }
                if (NPC.ai[2] == 1f)
                {
                    if (NPC.collideY || NPC.collideX)
                    {
                        NPC.ai[2] = 0f;
                        NPC.ai[3] = 0f;
                        NPC.netUpdate = true;
                    }
                    else if (NPC.wet)
                    {
                        NPC.velocity.Y -= 0.4f;
                        if (NPC.velocity.Y < -6f)
                        {
                            NPC.velocity.Y = -6f;
                        }
                        NPC.rotation = NPC.velocity.Y * (float)NPC.direction * 0.3f;
                        if (NPC.rotation < (float)Math.PI * -2f / 5f)
                        {
                            NPC.rotation = (float)Math.PI * -2f / 5f;
                        }
                        if (NPC.rotation > (float)Math.PI * 2f / 5f)
                        {
                            NPC.rotation = (float)Math.PI * 2f / 5f;
                        }
                        if (NPC.ai[3] == 1f)
                        {
                            NPC.ai[2] = 0f;
                            NPC.ai[3] = 0f;
                            NPC.netUpdate = true;
                        }
                    }
                    else
                    {
                        NPC.rotation += (float)NPC.direction * 0.2f;
                        NPC.ai[3] = 1f;
                        NPC.velocity.Y += 0.3f;
                        if (NPC.velocity.Y > 10f)
                        {
                            NPC.velocity.Y = 10f;
                        }
                    }
                    return;
                }
                if (NPC.ai[2] == 2f)
                {
                    if (NPC.collideY || NPC.collideX)
                    {
                        NPC.ai[2] = 0f;
                        NPC.ai[3] = 0f;
                        NPC.netUpdate = true;
                    }
                    else if (NPC.wet)
                    {
                        NPC.velocity.Y -= 0.4f;
                        if (NPC.velocity.Y < -6f)
                        {
                            NPC.velocity.Y = -6f;
                        }
                        NPC.rotation = NPC.velocity.Y * (float)NPC.direction * 0.3f;
                        if (NPC.rotation < (float)Math.PI * -2f / 5f)
                        {
                            NPC.rotation = (float)Math.PI * -2f / 5f;
                        }
                        if (NPC.rotation > (float)Math.PI * 2f / 5f)
                        {
                            NPC.rotation = (float)Math.PI * 2f / 5f;
                        }
                        if (Collision.GetWaterLine(NPC.Top.ToTileCoordinates(), out var waterLineHeight))
                        {
                            float y2 = waterLineHeight + 0f - NPC.position.Y;
                            NPC.velocity.Y = y2;
                            NPC.velocity.Y = MathHelper.Clamp(NPC.velocity.Y, -2f, 0.5f);
                            NPC.rotation = -(float)Math.PI / 5f * (float)NPC.direction;
                            NPC.velocity.X *= 0.95f;
                            if (NPC.ai[3] == 0f)
                            {
                                NPC.netUpdate = true;
                            }
                            NPC.ai[3]++;
                            if (NPC.ai[3] >= 300f)
                            {
                                NPC.ai[2] = 0f;
                                NPC.ai[3] = 0f;
                                NPC.netUpdate = true;
                                NPC.velocity.Y = 4f;
                            }
                            if (NPC.ai[3] == 60f && Main.rand.Next(2) == 0)
                            {
                                SoundEngine.PlaySound(SoundID.Dolphin, NPC.position);
                            }
                        }
                    }
                    else
                    {
                        NPC.ai[2] = 0f;
                        NPC.ai[3] = 0f;
                        NPC.netUpdate = true;
                        NPC.velocity.Y += 0.3f;
                        if (NPC.velocity.Y > 10f)
                        {
                            NPC.velocity.Y = 10f;
                        }
                    }
                    return;
                }
            }
            if (NPC.wet)
            {
                bool flag13 = false;
                if (false && NPC.type != 55 && NPC.type != 592 && NPC.type != 607 && NPC.type != 615)
                {
                    NPC.TargetClosest(faceTarget: false);
                    if (Main.player[NPC.target].wet && !Main.player[NPC.target].dead && Collision.CanHit(NPC.position, NPC.width, NPC.height, Main.player[NPC.target].position, Main.player[NPC.target].width, Main.player[NPC.target].height))
                    {
                        flag13 = true;
                    }
                }
                int num788 = (int)NPC.Center.X / 16;
                int num789 = (int)(NPC.position.Y + (float)NPC.height) / 16;
                if (Main.tile[num788, num789].TopSlope)
                {
                    if (Main.tile[num788, num789].LeftSlope)
                    {
                        NPC.direction = -1;
                        NPC.velocity.X = Math.Abs(NPC.velocity.X) * -1f;
                    }
                    else
                    {
                        NPC.direction = 1;
                        NPC.velocity.X = Math.Abs(NPC.velocity.X);
                    }
                }
                else if (Main.tile[num788, num789 + 1].TopSlope)
                {
                    if (Main.tile[num788, num789 + 1].LeftSlope)
                    {
                        NPC.direction = -1;
                        NPC.velocity.X = Math.Abs(NPC.velocity.X) * -1f;
                    }
                    else
                    {
                        NPC.direction = 1;
                        NPC.velocity.X = Math.Abs(NPC.velocity.X);
                    }
                }
                if (!flag13)
                {
                    if (NPC.collideX)
                    {
                        NPC.velocity.X *= -1f;
                        NPC.direction *= -1;
                        NPC.netUpdate = true;
                    }
                    if (NPC.collideY)
                    {
                        NPC.netUpdate = true;
                        if (NPC.velocity.Y > 0f)
                        {
                            NPC.velocity.Y = Math.Abs(NPC.velocity.Y) * -1f;
                            NPC.directionY = -1;
                            NPC.ai[0] = -1f;
                        }
                        else if (NPC.velocity.Y < 0f)
                        {
                            NPC.velocity.Y = Math.Abs(NPC.velocity.Y);
                            NPC.directionY = 1;
                            NPC.ai[0] = 1f;
                        }
                    }
                }

                if (flag13)
                {
                    NPC.TargetClosest();
                    if (NPC.type == 157)
                    {
                        if (NPC.velocity.X > 0f && NPC.direction < 0)
                        {
                            NPC.velocity.X *= 0.95f;
                        }
                        if (NPC.velocity.X < 0f && NPC.direction > 0)
                        {
                            NPC.velocity.X *= 0.95f;
                        }
                        NPC.velocity.X += (float)NPC.direction * 0.25f;
                        NPC.velocity.Y += (float)NPC.directionY * 0.2f;
                        if (NPC.velocity.X > 8f)
                        {
                            NPC.velocity.X = 7f;
                        }
                        if (NPC.velocity.X < -8f)
                        {
                            NPC.velocity.X = -7f;
                        }
                        if (NPC.velocity.Y > 5f)
                        {
                            NPC.velocity.Y = 4f;
                        }
                        if (NPC.velocity.Y < -5f)
                        {
                            NPC.velocity.Y = -4f;
                        }
                    }
                
                    else
                    {
                        NPC.velocity.X += (float)NPC.direction * 0.1f;
                        NPC.velocity.Y += (float)NPC.directionY * 0.1f;
                        if (NPC.velocity.X > 3f)
                        {
                            NPC.velocity.X = 3f;
                        }
                        if (NPC.velocity.X < -3f)
                        {
                            NPC.velocity.X = -3f;
                        }
                        if (NPC.velocity.Y > 2f)
                        {
                            NPC.velocity.Y = 2f;
                        }
                        if (NPC.velocity.Y < -2f)
                        {
                            NPC.velocity.Y = -2f;
                        }
                    }
                }
                else
                {
                    if (NPC.type == 157)
                    {
                        if (Main.player[NPC.target].position.Y > NPC.position.Y)
                        {
                            NPC.directionY = 1;
                        }
                        else
                        {
                            NPC.directionY = -1;
                        }
                        NPC.velocity.X += (float)NPC.direction * 0.2f;
                        if (NPC.velocity.X < -2f || NPC.velocity.X > 2f)
                        {
                            NPC.velocity.X *= 0.95f;
                        }
                        if (NPC.ai[0] == -1f)
                        {
                            float num790 = -0.6f;
                            if (NPC.directionY < 0)
                            {
                                num790 = -1f;
                            }
                            if (NPC.directionY > 0)
                            {
                                num790 = -0.2f;
                            }
                            NPC.velocity.Y -= 0.02f;
                            if (NPC.velocity.Y < num790)
                            {
                                NPC.ai[0] = 1f;
                            }
                        }
                        else
                        {
                            float num791 = 0.6f;
                            if (NPC.directionY < 0)
                            {
                                num791 = 0.2f;
                            }
                            if (NPC.directionY > 0)
                            {
                                num791 = 1f;
                            }
                            NPC.velocity.Y += 0.02f;
                            if (NPC.velocity.Y > num791)
                            {
                                NPC.ai[0] = -1f;
                            }
                        }
                    }
                    else
                    {
                        NPC.velocity.X += (float)NPC.direction * 0.1f;
                        float num792 = 1f;
                        if (NPC.type == 615)
                        {
                            num792 = 3f;
                        }
                        if (NPC.velocity.X < 0f - num792 || NPC.velocity.X > num792)
                        {
                            NPC.velocity.X *= 0.95f;
                        }
                        if (NPC.ai[0] == -1f)
                        {
                            NPC.velocity.Y -= 0.01f;
                            if ((double)NPC.velocity.Y < -0.3)
                            {
                                NPC.ai[0] = 1f;
                            }
                        }
                        else
                        {
                            NPC.velocity.Y += 0.01f;
                            if ((double)NPC.velocity.Y > 0.3)
                            {
                                NPC.ai[0] = -1f;
                            }
                        }
                    }
                    int num793 = (int)(NPC.position.X + (float)(NPC.width / 2)) / 16;
                    int num794 = (int)(NPC.position.Y + (float)(NPC.height / 2)) / 16;
                    if (Main.tile[num793, num794 - 1].LiquidAmount > 128)
                    {
                        if (Main.tile[num793, num794 + 1].HasTile)
                        {
                            NPC.ai[0] = -1f;
                        }
                        else if (Main.tile[num793, num794 + 2].HasTile)
                        {
                            NPC.ai[0] = -1f;
                        }
                    }
                    if (NPC.type != 157 && ((double)NPC.velocity.Y > 0.4 || (double)NPC.velocity.Y < -0.4))
                    {
                        NPC.velocity.Y *= 0.95f;
                    }
                }
            }
            else
            {
                if (NPC.velocity.Y == 0f)
                {
                    if (NPC.type == 65)
                    {
                        NPC.velocity.X *= 0.94f;
                        if ((double)NPC.velocity.X > -0.2 && (double)NPC.velocity.X < 0.2)
                        {
                            NPC.velocity.X = 0f;
                        }
                    }
                    else if (Main.netMode != 1)
                    {
                        NPC.velocity.Y = (float)Main.rand.Next(-50, -20) * 0.1f;
                        NPC.velocity.X = (float)Main.rand.Next(-20, 20) * 0.1f;
                        NPC.netUpdate = true;
                    }
                }
                NPC.velocity.Y += 0.3f;
                if (NPC.velocity.Y > 10f)
                {
                    NPC.velocity.Y = 10f;
                }
                NPC.ai[0] = 1f;
            }
            NPC.rotation = NPC.velocity.Y * (float)NPC.direction * 0.1f;
            if ((double)NPC.rotation < -0.2)
            {
                NPC.rotation = -0.2f;
            }
            if ((double)NPC.rotation > 0.2)
            {
                NPC.rotation = 0.2f;
            }
            return;*/
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.spriteDirection = NPC.direction;

            int frameCount = 6;
            int frameOffset = 0;

            if (NPC.wet)
            {
                frameCount = 6;
                frameOffset = 0;
            }

            else
            {
                frameCount = 2;
                frameOffset = 6;
            }

            NPC.frame.Y = (((int)NPC.frameCounter % 30 * frameCount) / 30 + frameOffset) * frameHeight;

            NPC.frameCounter++;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
			
        }

        public override void OnKill()
        {

        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
            //npcLoot.Add(ItemDropRule.Common(ModContent.ItemNPC.type<Items.Materials.LifeMatter>(), 1, 1, 3));
        }
	}
}