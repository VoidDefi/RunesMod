using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace RunesMod.CastingAnimations
{
    public class RaisingHandsAnimation : CastingAnimation
    {
        public override void Update(Player player, float progress)
        {
            if (Main.MouseWorld.X > player.Center.X) player.direction = 1;
            else player.direction = -1;

            float angle = Utils.AngleLerp(0, MathF.PI, -MathF.Pow(progress - 1, 6) + 1);
            angle *= -player.direction;

            player.SetCompositeArmFront(true, ArmFull, angle);
            player.SetCompositeArmBack(true, ArmFull, angle);
        }
    }
}
