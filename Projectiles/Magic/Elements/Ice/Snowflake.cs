using Microsoft.Xna.Framework;
using RunesMod.DamageClasses;
using RunesMod.MagicSchools.Elemental;
using RunesMod.ModUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace RunesMod.Projectiles.Magic.Elements.Ice
{
    public class Snowflake : ModProjectile
    {
        public static int maxTimeLeft = 60 * 2;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            maxTimeLeft = 60 * 2;
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.damage = 5;
            Projectile.timeLeft = maxTimeLeft;
            Projectile.tileCollide = true;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = ModContent.GetInstance<TrueMagic>();

            Projectile.MagicSchool().Add<IceMagic>();

            Projectile.scale = 0f;
            Projectile.alpha = 255;
            DrawOffsetX = -9; 
            DrawOriginOffsetY = -9;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Projectile.frame = Main.rand.Next(0, 2);
        }

        int timer;

        public override void AI()
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return;

            const int fadeTime = 8;

            //Fade In
            if (timer <= fadeTime)
            {
                Projectile.scale += 0.05f;
                Projectile.alpha -= 25;
            }

            //Fade Out
            if (timer >= maxTimeLeft - fadeTime)
            {
                Projectile.scale -= 0.02f;
                Projectile.alpha += 25;
            }

            if (Projectile.ai[0] != 1f)
            {
                Projectile.rotation += 0.05f;
                
                //Sine Falling
                Projectile.velocity.Y = MathHelper.Lerp(0, 3f, (Math.Clamp(timer, 0, 10)) / 10f);
                Projectile.velocity.X = (float)Math.Sin((float)timer / 10f) * 1.5f;
            }

            else
            {
                Projectile.velocity.X = 0;
                Projectile.velocity.Y = 0;

                Projectile.position.Y += 0.2f;
            }

            Projectile.width = 16;
            Projectile.height = 16;

            if (Main.rand.Next(0, 100) < 5 * 1f - (Projectile.alpha / 256f))
            {
                Vector2 position = Projectile.position;
                position.X += Main.rand.Next(Projectile.width);
                position.Y += Main.rand.Next(Projectile.height);

                Dust dust;
                dust = Dust.NewDustPerfect(position, 263, Vector2.Zero, 0, new Color(255, 255, 255), 0.8f);
                dust.noGravity = true;
            }

            timer++;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255, 255, 255, 150) * (1f - (Projectile.alpha / 256f));
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.ai[0] = 1f;
            Projectile.damage = 0;

            return false;
        }
    }
}
