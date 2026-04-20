using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using RunesMod.ModUtils;

namespace RunesMod.NPCs.Bosses.DeadGuard
{
    public partial class DeadGuardHand
    {
        #region Enums

        public enum HandTypes
        {
            Sword = -1,
            Axe = 1,
        }

        #endregion

        public static int BodyType => ModContent.NPCType<DeadGuard>();

        public int BodyIndex
        {
            get => (int)NPC.ai[0];
            set => NPC.ai[0] = value;
        }

        public NPC Body => HasBody ? Main.npc[BodyIndex] : null;

        public DeadGuard BodyModNPC => Body.type == BodyType ? Body.ModNPC as DeadGuard : null;

        public bool HasBody => BodyIndex > -1;

        public HandTypes HandType
        {
            get => (HandTypes)NPC.ai[1];
            set => NPC.ai[1] = (float)value;
        }

        public int HandBaseDirection => (int)HandType;

        public float Timer
        {
            get => NPC.ai[2];
            set => NPC.ai[2] = value;
        }

        public Vector2 AttackTarget;

        public Vector2 StartAttack;

        public Vector2 EndAttack;

        public int BaseAttackTime;

        public bool IsStartFromEnd;

        public int AttackCount;

        public override void AI()
        {
            if (DeSpawn())
            {
                return;
            }

            NPC.direction = HandBaseDirection;
            NPC.spriteDirection = NPC.direction;

            NPC.hide = Body.hide;
            NPC.immortal = Body.immortal;
            NPC.friendly = Body.friendly;

            if (BodyModNPC.State == DeadGuard.States.Teleportation)
            {
                NPC.Center = Body.Center;
            }

            FollowBody();

            Player player = Main.player[NPC.target];

            if (BodyModNPC.State == DeadGuard.States.HandsAttack)
            {
                int cooldown = (HandType == HandTypes.Sword ? 20 : 40);
                int attackTime = BaseAttackTime;
                int maxAttackCount = (HandType == HandTypes.Sword ? 6 : 4);

                if (AttackCount <= maxAttackCount)
                {
                    if (Timer > cooldown)
                    {
                        float factor = (Timer - cooldown) / attackTime;
                        factor = MathF.Pow(factor, 2);

                        Vector2 curve = Utilities.Curve(StartAttack, AttackTarget, EndAttack, factor);
                        Vector2 nextCurve = Utilities.Curve(StartAttack, AttackTarget, EndAttack, factor + 0.1f);

                        Vector2 oldCenter = NPC.Center;

                        NPC.Center = curve;
                        NPC.rotation = NPC.Center.AngleTo(nextCurve) + MathF.PI;
                        NPC.spriteDirection = 1;

                        Timer++;

                        if (factor > 1)
                        {
                            NPC.rotation = 0;
                            NPC.velocity = curve.DirectionTo(nextCurve) * attackTime;
                            IsStartFromEnd = !IsStartFromEnd;
                            NPC.spriteDirection = -1;
                            AttackCount++;
                            Timer = 0;
                        }
                    }

                    else
                    {
                        if (HandType == HandTypes.Sword && BodyModNPC.HandAttackState == HandTypes.Sword)
                        {
                            NPC.TargetClosest();

                            Vector2 direction = NPC.Center.DirectionTo(player.Center);
                            float distance = NPC.Center.Distance(player.Center) * 1.2f;

                            NPC.velocity *= 0.8f;
                            AttackTarget = player.Center + (direction * distance);
                            StartAttack = NPC.Center;

                            if (!IsStartFromEnd)
                                EndAttack = Body.Center;

                            else
                            {
                                EndAttack = Body.Center;
                                EndAttack.X -= 100;
                                EndAttack.Y -= 100;
                            }

                            BaseAttackTime = (int)(NPC.Center.Distance(player.Center) / 8f);

                            NPC.rotation = Utils.AngleLerp(NPC.rotation, 0, 0.2f);

                            Timer++;
                        }

                        else if (HandType == HandTypes.Axe && BodyModNPC.HandAttackState == HandTypes.Axe)
                        {
                            NPC.TargetClosest();

                            Vector2 direction = NPC.Center.DirectionTo(player.Center);
                            float distance = NPC.Center.Distance(player.Center) * 1.2f;

                            NPC.velocity *= 0.8f;
                            AttackTarget = player.Center + (direction * distance);
                            StartAttack = NPC.Center;

                            if (!IsStartFromEnd)
                                EndAttack = Body.Center;

                            else
                            {
                                EndAttack = Body.Center;
                                EndAttack.X -= 100;
                                EndAttack.Y -= 100;
                            }

                            BaseAttackTime = (int)(NPC.Center.Distance(player.Center) / 4f);

                            NPC.rotation = Utils.AngleLerp(NPC.rotation, 0, 0.2f);

                            Timer++;
                        }
                    }
                }

                else
                {
                    Timer++;

                    if (Timer > 60)
                    {
                        if (BodyModNPC.HandAttackState == HandTypes.Sword)
                            BodyModNPC.HandAttackState = HandTypes.Axe;

                        else if (BodyModNPC.HandAttackState == HandTypes.Axe)
                            BodyModNPC.HandAttackState = HandTypes.Sword;

                        Timer = 0;
                        AttackCount = 0;
                    }
                }
            }

            else
            {
                IsStartFromEnd = false;
                Timer = 0;
                AttackCount = 0;
                NPC.rotation = 0;
                NPC.spriteDirection = NPC.direction;
            }

            /*if (BodyModNPC.State == DeadGuard.States.Moving)
            {
                if (HandType == HandTypes.Axe)
                {
                    Point size = new Point(30, 30);
                    Vector2 collisionPos = NPC.Center - size.ToVector2();
                    if (Collision.SolidCollision(collisionPos, size.X, size.Y))
                    {
                        NPC.velocity.Y -= 1.51f;
                    }

                    else
                    {
                        NPC.velocity.Y += 1.5f;
                    }

                    NPC.velocity.X *= 0.95f;
                }
            }*/
        }

        private bool DeSpawn()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient && (!HasBody || !Body.active || Body.type != BodyType))
            {
                NPC.active = false;
                NPC.life = 0;
                NetMessage.SendData(MessageID.SyncNPC, number: NPC.whoAmI);
                return true;
            }
            return false;
        }

        private void FollowBody()
        {
            float factor = 0.6f;

            if (NPC.position.Y > Body.position.Y - 50f)
            {
                if (NPC.velocity.Y > 0f)
                {
                    NPC.velocity.Y *= factor;
                }
                NPC.velocity.Y -= 0.2f;
                if (NPC.velocity.Y > 6f)
                {
                    NPC.velocity.Y = 6f;
                }
            }
            else if (NPC.position.Y < Body.position.Y - 50f)
            {
                if (NPC.velocity.Y < 0f)
                {
                    NPC.velocity.Y *= factor;
                }
                NPC.velocity.Y += 0.2f;
                if (NPC.velocity.Y < -6f)
                {
                    NPC.velocity.Y = -6f;
                }
            }
            if (NPC.position.X + NPC.width / 2f > Body.position.X + (float)(Body.width / 2) - 120f * HandBaseDirection)
            {
                if (NPC.velocity.X > 0f)
                {
                    NPC.velocity.X *= factor;
                }
                NPC.velocity.X -= 0.1f;
                if (NPC.velocity.X > 8f)
                {
                    NPC.velocity.X = 8f;
                }
            }
            if (NPC.position.X + (NPC.width / 2f) < Body.position.X + (float)(Body.width / 2) - 120f * HandBaseDirection)
            {
                if (NPC.velocity.X < 0f)
                {
                    NPC.velocity.X *= factor;
                }
                NPC.velocity.X += 0.1f;
                if (NPC.velocity.X < -8f)
                {
                    NPC.velocity.X = -8f;
                }
            }
        }
    }
}
