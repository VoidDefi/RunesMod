using System.Collections.Generic;
using Terraria.ModLoader;

namespace RunesMod.CastingAnimations
{
    public class CastingAnimationLoader
    {
        private static int nextAnimation = 0;

        internal static readonly List<CastingAnimation> animations = new List<CastingAnimation>();

        public static int AnimationCount => nextAnimation;

        internal static int ReserveAnimationID()
        {
            int reserveID = nextAnimation;
            nextAnimation++;
            return reserveID;
        }

        public static CastingAnimation GetAnimation(int type)
        {
            if (type < 0 || type >= AnimationCount)
                return null;

            return animations[type];
        }

        public static CastingAnimation GetAnimation<T>() where T : CastingAnimation
        {
            return GetAnimation(AnimationType<T>());
        }

        public static int AnimationType<T>() where T : CastingAnimation
        {
            return ModContent.GetInstance<T>()?.Type ?? -1;
        }
    }
}
