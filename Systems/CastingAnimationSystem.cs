using RunesMod.CastingAnimations;
using System;
using Terraria;
using Terraria.ModLoader;

namespace RunesMod.Systems
{
    public class CastingAnimationSystem : ModPlayer
    {
        public int AnimationTime { get; private set; } = 0;

        public int AnimationMaxTime { get; private set; } = 0;

        public CastingAnimation CurrentAnimation { get; private set; } = null;

        public void SetAnimation(int time, int type)
        {
            AnimationMaxTime = time;
            AnimationTime = time;
            CurrentAnimation = CastingAnimationLoader.GetAnimation(type);

            if (CurrentAnimation == null)
            {
                AnimationMaxTime = 0;
                AnimationTime = 0;
            }
        }

        public override void PostUpdate()
        {
            if (CurrentAnimation == null || AnimationMaxTime == 0)
                return;

            if (!Player.ItemAnimationEndingOrEnded || Player.dead)
            {
                AnimationMaxTime = 0;
                AnimationTime = 0;
                CurrentAnimation = null;

                return;
            }

            if (AnimationTime > 0)
            {
                float progress = (float)(AnimationMaxTime - AnimationTime) / (float)AnimationMaxTime;

                CurrentAnimation.Update(Player, progress);

                AnimationTime--;
            }

            else
            {
                AnimationMaxTime = 0;
                CurrentAnimation = null;
            }

            base.PostUpdate();
        }
    }
}
