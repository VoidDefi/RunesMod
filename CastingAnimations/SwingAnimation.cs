using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace RunesMod.CastingAnimations
{
    public class SwingAnimation : CastingAnimation
    {
        public override void Update(Player player, float progress)
        {
            if (Main.MouseWorld.X > player.Center.X) player.direction = 1;
            else player.direction = -1;

            float angle = player.Center.AngleTo(Main.MouseWorld) - (MathF.PI / 2f);
            float offset = Utils.AngleLerp(-MathF.PI / 2.5f, MathF.PI / 2.5f, MathF.Pow(progress * 2 - 1, 2));

            angle += offset * player.direction;

            player.SetCompositeArmFront(true, ArmFull, angle);
            player.SetCompositeArmBack(true, ArmFull, angle);
        }
    }
}
