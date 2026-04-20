using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace RunesMod.CastingAnimations
{
    public class RotateAnimation : CastingAnimation
    {
        private static Player.CompositeArmStretchAmount[] Arms => new Player.CompositeArmStretchAmount[]
        {
            ArmFull,
            ArmQuarter,
            ArmNone
        };

        public override void Update(Player player, float progress)
        {
            if (Main.MouseWorld.X > player.Center.X) player.direction = 1;
            else player.direction = -1;

            float angle = player.Center.AngleTo(Main.MouseWorld) - (MathF.PI / 2f);
            float offset = MathF.Sin((float)Main.gameTimeCache.TotalGameTime.TotalSeconds * 10f) * 0.4f;

            int armStyle = 0;

            if (progress > 0.25 && progress < 0.75) armStyle = 0;
            if (progress < 0.25 || progress > 0.75) armStyle = 1;
            if (progress < 0.1 && progress > 0.9) armStyle = 2;

            player.SetCompositeArmFront(true, Arms[armStyle], angle + offset);
            player.SetCompositeArmBack(true, Arms[Arms.Length - 1 - armStyle], angle - offset);
        }
    }
}
