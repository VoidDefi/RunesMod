using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace RunesMod.CastingAnimations
{
    public class PushAnimation : CastingAnimation
    {
        public override void Update(Player player, float progress)
        {
            if (Main.MouseWorld.X > player.Center.X) player.direction = 1;
            else player.direction = -1;

            float angle = player.Center.AngleTo(Main.MouseWorld) - (MathF.PI / 2f);
            float offset = MathF.Sin((float)Main.gameTimeCache.TotalGameTime.TotalSeconds * 10f) * 0.1f;

            Player.CompositeArmStretchAmount armStyle = ArmNone;

            if (progress > 0.3) armStyle = ArmQuarter;
            if (progress > 0.4) armStyle = ArmThreeQuarters;
            if (progress > 0.45) armStyle = ArmFull;

            player.SetCompositeArmFront(true, armStyle, angle + offset);
            player.SetCompositeArmBack(true, armStyle, angle - offset);
        }
    }
}
