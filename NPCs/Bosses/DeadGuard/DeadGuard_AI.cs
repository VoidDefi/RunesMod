using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace RunesMod.NPCs.Bosses.DeadGuard
{
    public partial class DeadGuard
    {
        public static float TeleportationDistance => (50 - 10) * 16;
        
        public enum States
        {
            Raise,              // 1   Phase +
            PreTeleportation,   // Any Phase +
            Teleportation,      // Any Phase +
            HandsAttack,        // 1   Phase
            Moving,             // Any Phase +
            JumpAttack,         // Any Phase

        }

        public States State
        {
            get => (States)NPC.ai[0];
            set => NPC.ai[0] = (float)value;
        }

        public bool Transferring
        {
            get =>
            State == States.Raise ||
            State == States.PreTeleportation ||
            State == States.Teleportation;
        }

        public float Timer
        {
            get => NPC.ai[1];
            set => NPC.ai[1] = value;
        }

        public float AttackTimer
        {
            get => NPC.ai[2];
            set => NPC.ai[2] = value;
        }

        public DeadGuardHand.HandTypes HandAttackState = DeadGuardHand.HandTypes.Sword;

        public override void AI()
        {
            // This should almost always be the first code in AI() as it is responsible for finding the proper player target
            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
            {
                NPC.TargetClosest();
            }

            Player player = Main.player[NPC.target];

            if (player.dead)
            {
                NPC.noTileCollide = true;
                NPC.velocity.Y -= 0.04f;
                NPC.EncourageDespawn(10);
                return;
            }

            Raise(player);

            if (!Transferring)
            {
                if (MathF.Abs(NPC.Center.X - player.Center.X) > TeleportationDistance)
                {
                    Timer++;
                    if (Timer > 10)
                    {
                        Timer = 0;
                        State = States.PreTeleportation;
                    }
                }
            }

            Teleportation(player);

            if (State == States.Moving)
            {
                NPC.velocity.X += player.Center.X > NPC.Center.X ? 0.1f : -0.1f;
                NPC.velocity.X = Math.Clamp(NPC.velocity.X, -3f, 3f);
                NPC.direction = NPC.velocity.X >= 0 ? -1 : 1;
                NPC.spriteDirection = NPC.direction;

                JumpOverObstacles();

                AttackTimer++;

                if (AttackTimer > 5 * 60)
                {
                    NPC.velocity.X = 0;
                    AttackTimer = 0;
                    Timer = 0;
                    State = States.HandsAttack;
                }
            }

            if (State == States.HandsAttack)
            {
                
            }
        }

        private void Teleportation(Player player)
        {
            if (State == States.PreTeleportation)
            {
                NPC.noTileCollide = true;

                if (Collision.SolidCollision(NPC.position, NPC.width, NPC.height))
                {
                    SetProperties(true);
                    NPC.velocity.X = 0;
                    NPC.velocity.Y += 0.1f;
                    NPC.velocity.Y = Math.Clamp(NPC.velocity.Y, 0.1f, 4f);

                    Timer++;
                    if (Timer > 40) 
                    {
                        int x = (int)(player.Center.X / 16);
                        int y = (int)(player.Center.Y / 16);

                        int foundY = -1;
                        for (int i = y; i < Main.maxTilesY; i++)
                        {
                            Tile tile = Framing.GetTileSafely(x, i);
                            if (tile.HasUnactuatedTile && Main.tileSolid[tile.TileType])
                            {
                                foundY = i;
                                break;
                            }
                        }

                        if (foundY > -1)
                        {
                            SetProperties(false);
                            Timer = 0;
                            float newY = (foundY + 6) * 16f;
                            NPC.Center = new Vector2(player.Center.X, newY);
                            State = States.Teleportation;
                        }
                    }
                }
            }

            void SetProperties(bool state)
            {
                //NPC.friendly = state;
                NPC.immortal = state;
                NPC.hide = state;
                NPC.noTileCollide = state;
                NPC.noGravity = state;
            }
        }

        private void Raise(Player player)
        {
            if (State == States.Raise || State == States.Teleportation)
            {
                SetProperties(true);
                NPC.velocity.X = 0;
                NPC.velocity.Y -= 0.1f;
                NPC.velocity.Y = Math.Clamp(NPC.velocity.Y, -4f, -0.1f);

                if (!Collision.SolidCollision(NPC.position, NPC.width, NPC.height))
                {
                    State = States.Moving;
                    NPC.velocity.Y = MathF.Abs(NPC.velocity.Y);
                    SetProperties(false);
                }
            }

            else SetProperties(false);

            void SetProperties(bool state)
            {
                //NPC.friendly = state;
                NPC.immortal = state;
                NPC.hide = state;
                NPC.noTileCollide = state;
                NPC.noGravity = state;
            }
        }

        private void JumpOverObstacles()
        {
            if (NPC.velocity.Y == 0 && !NPC.noTileCollide)
            {
                float MaxJumpForce = 8; //v

                bool canJump = false;
                float jumpForce = MaxJumpForce;

                Point tilePosition = (NPC.position / 16).ToPoint();
                tilePosition.X -= 1;

                int tilesWidth = (int)MathF.Ceiling(NPC.width / 16f) + 2;
                int tilesHeight = (int)MathF.Ceiling(NPC.height / 16f);

                int startOffsetX = NPC.direction < 0 ? tilesWidth / 2 : 0;
                int endOffsetX = NPC.direction > 0 ? tilesWidth / 2 : 0;

                for (int i = startOffsetX; i < tilesWidth - endOffsetX; i++)
                {
                    for (int j = 0; j < tilesHeight; j++)
                    {
                        int x = tilePosition.X + i;
                        int y = tilePosition.Y + j;

                        Tile tile = Framing.GetTileSafely(x, y);

                        if (tile.HasUnactuatedTile && Main.tileSolid[tile.TileType])
                        {
                            canJump = true;
                            jumpForce -= j;
                            break;
                        }
                    }
                }

                if (NPC.velocity.X != 0 && !canJump && jumpForce > 2f)
                {
                    bool halfBlock = false;
                    bool otherBlock = false;
                    bool isHole = true;

                    for (int i = startOffsetX; i < tilesWidth - endOffsetX; i++)
                    {
                        int x = tilePosition.X + i;
                        int y = tilePosition.Y + tilesHeight;

                        Tile tile = Framing.GetTileSafely(x, y);

                        if (tile.HasUnactuatedTile)
                        {
                            isHole = false;

                            if (tile.IsHalfBlock)
                                halfBlock = true;

                            else if (!tile.IsHalfBlock)
                                otherBlock = true;
                        }

                        if (halfBlock == true && otherBlock == true)
                        {
                            canJump = true;
                            jumpForce -= tilesWidth - 1f;
                            break;
                        }
                    }

                    if (isHole)
                    {
                        jumpForce = MaxJumpForce;
                        canJump = true;
                    }
                }

                if (canJump)//Collision.SolidCollision(NPC.position - new Vector2(1, 0), NPC.width + 2, NPC.height - 2))
                {
                    NPC.velocity.Y -= jumpForce;
                }
            }
        }
    }
}
