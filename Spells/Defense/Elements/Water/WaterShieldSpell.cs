using Microsoft.Xna.Framework;
using RunesMod.CastingAnimations;
using RunesMod.MagicSchools.Elemental;
using RunesMod.ModUtils;
using RunesMod.Shields;
using RunesMod.Shields.Elements.Water;
using Terraria;

namespace RunesMod.Spells.Defense.Elements.Water
{
    public class WaterShieldSpell : Spell
    {
        protected override void SetDefaults()
        {
            costType = CostTypes.Mana;
            cost = 10;

            cooldownTime = 20;
            animationTime = 20;
            animationStyle = CastingAnimationLoader.AnimationType<RaisingHandsAnimation>();

            flipPlayer = false;

            sustainable = true;
            occupiedSlotLength = 1;

            concentrationBarGradient = new
            (
                new(255, 237, 166), 
                new(255, 153, 0), 
                new(182, 55, 0), 
                new(134, 27, 0)
            );

            AddMagicSchool<WaterMagic>();
        }

        public override void OnCasting(Player player, Vector2 velocity)
        {
            if (player.whoAmI != Main.myPlayer) return;

            player.ShieldSystem().AddShield(ShieldLoader.ShieldType<WaterShield>(), this);
        }
    }
}
