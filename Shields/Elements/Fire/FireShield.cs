using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using RunesMod.ModUtils;
using System.Runtime.InteropServices;
using Terraria;
using Terraria.Graphics;
using Terraria.Audio;
using Terraria.ID;
using RunesMod.Spells;
using System;

namespace RunesMod.Shields.Elements.Fire
{
    public class FireShield : Shield
    {
        public float LightFactor => 0.75f + strength / (float)maxStrength / 4f;

        public Vector2 Center => Player.VisualPosition + Player.Size / 2;

        public override void SetDefault()
        {
            maxStrength = 50;
            destroyable = true;
        }

        public override void ModifyHurt(ref Player.HurtModifiers hurt, ref int damage)
        {
            if (damage <= strength)
            {
                if (Player.ConsumeMana(GetUseMana(damage)))
                {
                    AbsorbedEffect(damage);

                    hurt.Cancel();
                    Player.SetImmuneTimeForAllTypes(20);
                    Player.immuneNoBlink = true;
                    strength -= damage;
                }
            }

            else
            {
                if (Player.ConsumeMana(GetUseMana(strength)))
                {
                    AbsorbedEffect(strength);

                    damage -= strength;
                    strength = 0;
                }
            }
        }

        private static int GetUseMana(int damage)
        {
            return (int)Math.Clamp(damage / 4f, 1, float.MaxValue);
        }

        public override void Destroy()
        {
            SoundEngine.PlaySound(SoundID.Item89, Player.Center);

            Vector2 dustDirection = Vector2.One * 15f;

            int dustCount = Main.rand.Next(10, 22);
            int realDustCount = dustCount + Main.rand.Next(2, 7);

            float offsetAngle = 0.05f;
            float addedAngle = MathHelper.ToRadians(1f / dustCount * 360f);
            float angle = Main.rand.NextFloat(-offsetAngle, offsetAngle);

            for (int i = 0; i < realDustCount; i++)
            {
                float scale = Main.rand.NextFloat(1f, 4f);

                Vector2 position = Player.Center + new Vector2(0, 30f / 2f + Main.rand.NextFloat(0f, 30f / 2f)).RotatedBy(angle);

                Dust dust;
                dust = Main.dust[Dust.NewDust(position, 0, 0, 6, 0f, 0f, 0, new Color(255, 255, 255), scale)];
                dust.velocity = (dustDirection * Main.rand.NextFloat(0.1f, 0.3f)).RotatedBy(angle);

                dust.noGravity = true;

                angle += addedAngle + Main.rand.NextFloat(-offsetAngle, offsetAngle);
            }
        }

        public override void Update()
        {
            Lighting.AddLight(Center, 0.6f * LightFactor, 0.2f * LightFactor, 0f);

            /*foreach (Projectile projectile in Main.projectile)
            {
                if (projectile.active && projectile.hostile)
                {
                    float distance = projectile.Center.Distance(Player.Center);

                    if (distance < 50)
                    {
                        projectile.Kill();
                    }
                }
            }

            foreach (NPC npc in Main.npc)
            {
                if (npc.active)
                {
                    float distance = npc.Center.Distance(Player.Center);

                    if (distance < 50)
                    {
                        npc.velocity = Vector2.Zero;
                    }
                }
            }*/
        }

        public override void Draw(Camera camera)
        {
            if (Main.dedServ) return;

            SpriteBatch spriteBatch = camera.SpriteBatch;

            Texture2D circleMask = ModAssets.Request<Texture2D>(ModAssets.ShieldMaskTextures, "ShieldCircleMask").Value;
            Texture2D noise = ModAssets.Request<Texture2D>(ModAssets.NoiseTextures, "PlasmaNoise").Value;

            Effect shieldEffect = ModAssets.Request<Effect>(ModAssets.Effects, "Shield").Value;

            shieldEffect.Parameters["uNoise"].SetValue(noise);
            shieldEffect.Parameters["uNoiseSize"].SetValue(1f);
            shieldEffect.Parameters["uNoiseMoveDirection"].SetValue(new Vector2(0.05f * Main.gameTimeCache.TotalGameTime.Ticks, 0f) / 1000000f);
            shieldEffect.Parameters["uNoiseFactor"].SetValue(0.8f * LightFactor);
            shieldEffect.Parameters["uCircleColor"].SetValue(Color.White.ToVector3());
            shieldEffect.Parameters["uCircleFactor"].SetValue(0.8f * LightFactor);

            shieldEffect.Parameters["uColor"]?.SetValue(new Vector3(1.5f, 0f, 0f));
            shieldEffect.Parameters["uHotColor"]?.SetValue(new Vector3(1.5f, 0.8f, 0f));
            shieldEffect.Parameters["uHotFactor"]?.SetValue(3.5f);

            spriteBatch.Begin(shieldEffect);

            spriteBatch.Draw(circleMask, Center - camera.UnscaledPosition, null, Color.White, 0f, new(256f / 2f), 0.3f, 0, 0);

            spriteBatch.End();
        }
    }
}
