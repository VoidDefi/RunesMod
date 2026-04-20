using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RunesMod.ModUtils;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace RunesMod.Projectiles
{
    public class LifeCrystalFinder : ModProjectile
    {
        public enum AIState
        {
            Setup,
            ScanAnimation,
            Look,
            Fail,
            Found,
            Reset,
            Destruction
        }

        public static int MaxTimeLeft => 10 * 60 * 60;

        public static int AnimationDelay => 1;

        public static int SetupTime => 30;

        public static int ReFindTime => 10;

        public static int FindAreaSize => 150;

        public static float RotationSpeed => 50;

        public AIState State
        {
            get => (AIState)Projectile.ai[0];
            set => Projectile.ai[0] = (float)value;
        }

        public float Timer
        {
            get => Projectile.ai[3];
            set => Projectile.ai[3] = value;
        }

        public Vector2? Position
        {
            get
            {
                if (Projectile.ai[1] > 0 && Projectile.ai[2] > 0)
                    return new Vector2(Projectile.ai[1], Projectile.ai[2]);

                return null;
            }

            set
            {
                if (value.HasValue && value?.X > 0 && value?.Y > 0)
                {
                    Projectile.ai[1] = value.Value.X;
                    Projectile.ai[2] = value.Value.Y;
                }

                else
                {
                    Projectile.ai[1] = 0;
                    Projectile.ai[2] = 0;
                }
            }
        }

        public float WaveTimer
        {
            get => Projectile.ai[4];
            set => Projectile.ai[4] = value;
        }

        public float ScanProcess
        {
            get => Projectile.ai[5];
            set => Projectile.ai[5] = value;
        }

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.timeLeft = MaxTimeLeft;
            Projectile.tileCollide = false;
            Projectile.friendly = false;
            Projectile.hostile = false;

            Projectile.ai = new float[6];

            DrawOriginOffsetY = -10;
        }

        public override void AI()
        {
            Projectile.netUpdate = true;

            if (Projectile.owner == Main.myPlayer)
            {
                Player player = Main.player[Main.myPlayer];

                Vector2 target = player.Center + new Vector2(0, -50);

                float newX = MathHelper.Lerp(Projectile.Center.X, target.X, 0.4f);
                float newY = MathHelper.Lerp(Projectile.Center.Y, target.Y, 0.4f);

                Projectile.Center = new(newX, newY);
            }

            if (State == AIState.Setup)
            {
                Timer += 1f;

                if (Timer > SetupTime)
                {
                    State = AIState.ScanAnimation;
                    ScanProcess = 0;
                    Timer = 0;
                }
            }

            else if (State == AIState.Look)
            {
                FindLifeCrystal();

                if (Position == null)
                {
                    //Main.NewText("fail");
                    State = AIState.Fail;
                }

                else
                {
                    //Main.NewText("found");
                    State = AIState.Found;
                }
            }

            else if (State == AIState.Fail)
            {
                Timer += 1f;

                if (Timer > ReFindTime)
                {
                    State = AIState.ScanAnimation;
                    Timer = 0;
                }
            }

            else if (State == AIState.Found && Position.HasValue)
            {
                if (Timer > 30)
                {
                    float angleTo = Projectile.Center.AngleTo(Position.Value) + (MathF.PI / 2f);
                    float angle = Projectile.rotation;

                    Projectile.rotation = angle + MathHelper.WrapAngle(angleTo - angle) * 0.3f;

                    if (Projectile.Distance(Position.Value) < 16 * 6)
                    {
                        Timer = 0;
                        State = AIState.Destruction;
                    }
                }

                else Timer++;
            }

            else if (State == AIState.Reset)
            {
                Projectile.rotation = MathHelper.Lerp(Projectile.rotation, 0, 0.3f);

                if (Projectile.rotation < 0.01f)
                {
                    Projectile.rotation = 0;
                }

                if (Projectile.position.Distance(Projectile.oldPosition) < 0.005f)
                {
                    if (Timer > 30)
                    {
                        Timer = 0;
                        State = AIState.ScanAnimation;
                    }

                    Timer++;
                }
            }

            else if (State == AIState.ScanAnimation)
            {
                if (Timer < 10)
                {
                    Projectile.rotation += MathF.PI / RotationSpeed;
                    Timer++;
                }

                if (WaveTimer <= 0f)
                {
                    Timer = 0f;
                    WaveTimer = 1f;

                    if (ScanProcess >= 1 - 0.1f)
                    {
                        ScanProcess = 0;
                        State = AIState.Look;
                    }

                    else
                    {
                        float halfStep = 0.1f / 2f;
                        float added = ScanProcess == 0 || ScanProcess == halfStep ? halfStep : 0.1f;
                        ScanProcess += added;
                    }
                }

                if (Projectile.position.Distance(Projectile.oldPosition) >= 0.005f)
                {
                    ScanProcess = 0;
                    Timer = 0f;
                    State = AIState.Reset;
                }
            }

            else if(State == AIState.Destruction)
            {
                Timer++;

                Projectile.position += Main.rand.NextVector2Circular(2, 2) * Timer / 60;

                if (Timer > 60) Projectile.Kill();
            }

            Animation();
        }

        private void Animation()
        {
            if (Projectile.timeLeft % AnimationDelay == 0)
            {
                int frames = Main.projFrames[Projectile.type];

                if (++Projectile.frameCounter >= frames)
                {
                    Projectile.frameCounter = 0;
                    Projectile.frame = ++Projectile.frame % frames;
                }
            }

            if (WaveTimer > 0)
            {
                WaveTimer -= 0.02f;
                WaveTimer = Math.Clamp(WaveTimer, 0f, 1f);
            }
        }

        public override void OnKill(int timeLeft)
        {
            SoundStyle style = new SoundStyle("Terraria/Sounds/Item_14");
            SoundEngine.PlaySound(style, Projectile.position);

            float factor = 3f;

            int smokeCount = Main.rand.Next(3, 12);
            for (int i = 0; i < smokeCount; i++)
            {
                Vector2 velocity = Main.rand.NextVector2Circular(1, 1) * factor;
                int type = Main.rand.Next(GoreID.Smoke1, GoreID.Smoke3 + 1);

                Gore.NewGore(Projectile.GetSource_Death(), Projectile.position, velocity, type, 1f);
            }

            Gore.NewGore(Projectile.GetSource_Death(), Projectile.position, Main.rand.NextVector2Circular(1, 1) * factor, Mod.Find<ModGore>("Cog1Fragment1Gore").Type, 1f);
            Gore.NewGore(Projectile.GetSource_Death(), Projectile.position, Main.rand.NextVector2Circular(1, 1) * factor, Mod.Find<ModGore>("Cog1Fragment2Gore").Type, 1f);

            int brassCount = Main.rand.Next(3, 8);
            for (int i = 0; i < brassCount; i++)
            {
                int n = Main.rand.Next(1, 5);
                Vector2 velocity = Main.rand.NextVector2Circular(1, 1) * factor;
                Gore.NewGore(Projectile.GetSource_Death(), Projectile.position, velocity, Mod.Find<ModGore>($"BrassSmall{n}Gore").Type, 1f);
            }

            int lifeShardCount = Main.rand.Next(2, 5);
            for (int i = 0; i < lifeShardCount; i++)
            {
                int n = Main.rand.Next(1, 5);
                Vector2 velocity = Main.rand.NextVector2Circular(1, 1) * factor;
                Gore.NewGore(Projectile.GetSource_Death(), Projectile.position, velocity, Mod.Find<ModGore>($"LifeShard{n}Gore").Type, 1f);
            }

            int manaShardCount = Main.rand.Next(1, 3);
            for (int i = 0; i < manaShardCount; i++)
            {
                int n = Main.rand.Next(1, 5);
                Vector2 velocity = Main.rand.NextVector2Circular(1, 1) * factor;
                Gore.NewGore(Projectile.GetSource_Death(), Projectile.position, velocity, Mod.Find<ModGore>($"ManaShard{n}Gore").Type, 1f);
            }
        }

        public override void PostDraw(Color lightColor)
        {
            if (WaveTimer > 0)
            {
                Effect overlapping = ModAssets.Request<Effect>(ModAssets.Effects, "Overlapping").Value;
                Texture2D wave = ModAssets.Request<Texture2D>(ModAssets.ShieldMaskTextures, "ShieldCircleMask").Value;

                float waveSpeed = 100;
                float scale = 0.5f;
                Vector4 color    = new Vector4(1.5f, 0.1f, 0.2f, 1f) * WaveTimer;
                Vector4 hotColor = new Vector4(1.5f, 0.1f, 0.2f, 1f) * WaveTimer;

                overlapping.Parameters["uColor"].SetValue(color * 0.5f);
                overlapping.Parameters["uHotColor"].SetValue(hotColor);
                overlapping.Parameters["uHotFactor"].SetValue(1f);

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(overlapping);

                Vector2 drawPos = Projectile.Center - Main.screenPosition + (Vector2.UnitY.RotatedBy(Projectile.rotation - MathF.PI) * waveSpeed * (1f - WaveTimer));
                Vector2 origin = new Vector2(wave.Width * 0.5f, wave.Height * 0.5f);
                Main.EntitySpriteDraw(wave, drawPos, null, Color.White, Projectile.rotation, origin, new Vector2(1f, 0.5f) * scale * (1f - WaveTimer), 0f, 0f);

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(null);
            }
        }

        private void FindLifeCrystal()
        {
            Point center = (Projectile.Center / 16).ToPoint();
            int halfSize = FindAreaSize / 2;
            Rectangle area = new Rectangle(center.X - halfSize, center.Y - halfSize, FindAreaSize, FindAreaSize);

            Point findPosition = new Point(0, 0);
            float distance = float.MaxValue;

            for (int i = 0; i < area.Width; i++)
            {
                for (int j = 0; j < area.Height; j++)
                {
                    int x = Math.Clamp(i + area.X, 10, Main.maxTilesX - 10);
                    int y = Math.Clamp(j + area.Y, 10, Main.maxTilesY - 10);

                    Tile tile = Framing.GetTileSafely(x, y);
                    
                    if (tile.HasTile && (tile.TileType == TileID.Heart || tile.TileType == TileID.LifeCrystalBoulder))
                    {
                        float currentDistance = Utils.DistanceSQ(center.ToVector2(), new(x, y));
                        if (currentDistance < distance)
                        {
                            distance = currentDistance;
                            findPosition = new Point(x, y);
                        }
                    }
                }
            }

            if (findPosition.X > 0 && findPosition.Y > 0)
            {
                Position = findPosition.ToVector2() * 16f;
            }
        }
    }
}
