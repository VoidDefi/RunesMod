using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using RunesMod.Dusts;

namespace RunesMod.Projectiles
{
    public class GlowingPowder : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 48;
            Projectile.height = 48;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.alpha = 255;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            Projectile.velocity *= 0.95f;
            Projectile.ai[0] += 1f;

            if (Projectile.ai[0] == 180f) Projectile.Kill();

            if (Projectile.ai[1] == 0f)
            {
                Projectile.ai[1] = 1f;

                for (int i = 0; i < 30; i++)
                {
                    Dust.NewDust
                    (
                        Projectile.position, 
                        Projectile.width, 
                        Projectile.height,
                        ModContent.DustType<GlowingPowderDust>(),
                        Projectile.velocity.X, 
                        Projectile.velocity.Y, 
                        50
                    );
                }
            }

            if (Main.myPlayer == Projectile.owner)
            {
                int startX = (int)(Projectile.position.X / 16f) - 1;
                int startY = (int)(Projectile.position.Y / 16f) - 1;

                int endX = (int)((Projectile.position.X + Projectile.width) / 16f) + 2;
                int endY = (int)((Projectile.position.Y + Projectile.height) / 16f) + 2;
                
                if (startX < 0) startX = 0;
                if (startY < 0) startY = 0;

                if (endX > Main.maxTilesX) endX = Main.maxTilesX;
                if (endY > Main.maxTilesY) endY = Main.maxTilesY;

                Vector2 position = new Vector2();

                for (int i = startX; i < endX; i++)
                {
                    for (int j = startY; j < endY; j++)
                    {
                        position.X = i * 16;
                        position.Y = j * 16;

                        if 
                        (
                            !(Projectile.position.X + Projectile.width > position.X) || 
                            !(Projectile.position.X < position.X + 16f) || 
                            !(Projectile.position.Y + Projectile.height > position.Y) || 
                            !(Projectile.position.Y < position.Y + 16f) || 
                            !Main.tile[i, j].HasTile
                        )
                        {
                            continue;
                        }

                       WorldGen.Convert(i, j, 3, 1);
                    }
                }
            }

            Rectangle hitbox = Projectile.Hitbox;

            if (Main.netMode != 1)
            {
                for (int i = 0; i < 200; i++)
                {
                    NPC npc = Main.npc[i];

                    if (npc.active)
                    {
                        Rectangle npcHitbox = new((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height);
                        if (hitbox.Intersects(npcHitbox))
                        {
                            switch (npc.type)
                            {
                                case NPCID.Crab:
                                    npc.Transform(NPCID.AnomuraFungus);
                                    break;

                                case NPCID.LadyBug:
                                    npc.Transform(NPCID.MushiLadybug);
                                    break;

                                case NPCID.Snatcher:
                                case NPCID.ManEater:
                                    float x = npc.ai[0];
                                    float y = npc.ai[1];
                                    npc.Transform(NPCID.FungiBulb);
                                    npc.ai[0] = x;
                                    npc.ai[1] = y;
                                    break;

                                case NPCID.AngryTrapper:
                                    float x1 = npc.ai[0];
                                    float y1 = npc.ai[1];
                                    npc.Transform(NPCID.GiantFungiBulb);
                                    npc.ai[0] = x1;
                                    npc.ai[1] = y1;
                                    break;

                                case NPCID.Snail:
                                    npc.Transform(NPCID.GlowingSnail);
                                    break;

                                case NPCID.CaveBat:
                                case NPCID.JungleBat:
                                    npc.Transform(NPCID.SporeBat);
                                    break;

                                case NPCID.Skeleton:
                                case NPCID.SmallSkeleton:
                                case NPCID.BigSkeleton:

                                case NPCID.HeadacheSkeleton:
                                case NPCID.SmallHeadacheSkeleton:
                                case NPCID.BigHeadacheSkeleton:

                                case NPCID.MisassembledSkeleton:
                                case NPCID.SmallMisassembledSkeleton:
                                case NPCID.BigMisassembledSkeleton:

                                case NPCID.PantlessSkeleton:
                                case NPCID.SmallPantlessSkeleton:
                                case NPCID.BigPantlessSkeleton:

                                case NPCID.BoneThrowingSkeleton:
                                case NPCID.BoneThrowingSkeleton2:
                                case NPCID.BoneThrowingSkeleton3:
                                case NPCID.BoneThrowingSkeleton4:

                                case NPCID.SkeletonTopHat:
                                case NPCID.SkeletonAstonaut:
                                case NPCID.SkeletonAlien:

                                case NPCID.GreekSkeleton:

                                case NPCID.UndeadViking:
                                    npc.Transform(NPCID.SporeSkeleton);
                                    break;

                                case NPCID.Zombie:
                                case NPCID.SmallZombie:
                                case NPCID.BigZombie:
                                
                                case NPCID.BaldZombie:
                                case NPCID.SmallBaldZombie:
                                case NPCID.BigBaldZombie:

                                case NPCID.PincushionZombie:
                                case NPCID.SmallPincushionZombie:
                                case NPCID.BigPincushionZombie:

                                case NPCID.SlimedZombie:
                                case NPCID.SmallSlimedZombie:
                                case NPCID.BigSlimedZombie:

                                case NPCID.TwiggyZombie:
                                case NPCID.SmallTwiggyZombie:
                                case NPCID.BigTwiggyZombie:

                                case NPCID.FemaleZombie:
                                case NPCID.SmallFemaleZombie:
                                case NPCID.BigFemaleZombie:

                                case NPCID.ZombieDoctor:
                                case NPCID.ZombieSuperman:
                                case NPCID.ZombiePixie:

                                case NPCID.ZombieXmas:
                                case NPCID.ZombieSweater:

                                case NPCID.ZombieRaincoat:
                                case NPCID.SmallRainZombie:
                                case NPCID.BigRainZombie:

                                case NPCID.TorchZombie:

                                case NPCID.MaggotZombie:

                                case NPCID.ZombieEskimo:

                                case NPCID.ArmedTorchZombie:
                                case NPCID.ArmedZombie:
                                case NPCID.ArmedZombieCenx:
                                case NPCID.ArmedZombieEskimo:
                                case NPCID.ArmedZombiePincussion:
                                case NPCID.ArmedZombieSlimed:
                                case NPCID.ArmedZombieSwamp:
                                case NPCID.ArmedZombieTwiggy:
                                    if (Main.rand.NextBool())
                                        npc.Transform(NPCID.ZombieMushroom);

                                    else npc.Transform(NPCID.ZombieMushroomHat);

                                    break;
                            }

                            if (Main.hardMode)
                            {
                                if (npc.type == NPCID.BlueJellyfish ||
                                    npc.type == NPCID.PinkJellyfish ||
                                    npc.type == NPCID.GreenJellyfish ||
                                    npc.type == NPCID.BloodJelly)
                                {
                                    npc.Transform(NPCID.FungoFish);
                                }

                                else if (npc.type == NPCID.Worm)
                                {
                                    npc.Transform(NPCID.TruffleWorm);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
