using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RunesMod.DamageClasses;
using RunesMod.MagicSchools.Elemental;
using RunesMod.ModUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace RunesMod.Projectiles.Magic.Elements.Ice
{
    public class FrostyMist : ModProjectile
    {
        public static int maxTimeLeft = 800;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 30;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.width = 64;
            Projectile.height = 64;
            Projectile.damage = 5;
            Projectile.timeLeft = maxTimeLeft;
            Projectile.tileCollide = true;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = ModContent.GetInstance<TrueMagic>();

            Projectile.MagicSchool().Add<IceMagic>();
            Projectile.MagicSchool().Add<AirMagic>();

            Projectile.scale = 0.2f;
            Projectile.alpha = 255;
            DrawOffsetX = -Projectile.width / 2;
            DrawOriginOffsetY = -Projectile.height / 2;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Projectile.rotation = Main.rand.NextFloat(0, MathHelper.TwoPi);
        }

        int timer;

        public override void AI()
        {
            const int fadeTime = 8;

            float length = Projectile.velocity.Length();

            //Fade In
            if (timer <= fadeTime)
                Projectile.alpha -= 25;

            //Fade Out
            if (length < 2f)
                Projectile.alpha = (int)((1f - length / 2f) * 256);

            Projectile.rotation += 0.05f;
            Projectile.scale += 0.0055f;

            Projectile.velocity *= Projectile.ai[0] != 1f ? 0.95f : 0.8f;

            if (length >= 0 && length <= 0.5f) 
                Projectile.Kill();

            if (Main.rand.Next(0, 100) < 3)
            {
                Vector2 position = Projectile.Center;
                position.X += Main.rand.Next(-32, 33);
                position.Y += Main.rand.Next(-32, 33);

                Projectile.NewProjectile(Projectile.GetSource_FromAI(), position, Vector2.Zero, ModContent.ProjectileType<Snowflake>(), Projectile.damage / 5, 0f);
            }

            if (Main.rand.Next(0, 100) < 30 * Projectile.scale)
            {
                Vector2 position = Projectile.Center;
                position.X += Main.rand.Next(-52, 53) * Projectile.scale;
                position.Y += Main.rand.Next(-52, 53) * Projectile.scale;

                Dust dust;
                dust = Dust.NewDustPerfect(position, 263, Vector2.Zero, 0, new Color(255, 255, 255), 0.8f);
                dust.noGravity = true;
            }

            timer++;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return null;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity = oldVelocity;

            Projectile.ai[0] = 1f;
            Projectile.damage = 0;

            return false;
        }

        public override void PostDraw(Color lightColor)
        {
            Effect overlapping = ModAssets.Request<Effect>(ModAssets.Effects, "Overlapping").Value;
            Texture2D mistMask = ModAssets.Request<Texture2D>(ModAssets.MaskTextures, "MistMask").Value;

            float alpha = 1f - Projectile.alpha / 256f;

            float curAlpha = alpha - 0.4f;
            float curScale = Projectile.scale;

            for (int i = -1; i < Projectile.oldPos.Length; i++)
            {
                curAlpha -= 0.005f;
                curScale -= 0.008f;

                if (curScale < 0) curScale = 0;

                Vector2 position = Projectile.Center;
                if (i >= 0) position = Projectile.oldPos[i];

                if (i < Projectile.oldPos.Length - 1)
                    if (position.Distance(Projectile.oldPos[i + 1]) < 1f)
                        continue;

                overlapping.Parameters["uColor"].SetValue(new Vector4(0.0f, 0.8f, 1f, 0.1f) * 6 * curAlpha);
                overlapping.Parameters["uHotColor"].SetValue(new Vector4(1, 1, 1, 0.01f) * 12 * curAlpha);
                overlapping.Parameters["uHotFactor"].SetValue(2f);

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(overlapping);

                Vector2 drawPos = position - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
                Vector2 origin = new Vector2(mistMask.Width * 0.5f, mistMask.Height * 0.5f);
                Main.EntitySpriteDraw(mistMask, drawPos, null, Color.White, Projectile.rotation + (i << 2 ^ 100), origin, curScale * 4f, 0f, 0f);

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(null);
            }

            base.PostDraw(lightColor);
        }
    }
}
