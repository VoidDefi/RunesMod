using rail;
using RunesMod.MagicSchools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace RunesMod.CastingAnimations
{
    public class CastingAnimation : ModType
    {
        #region Arm Styles

        public static Player.CompositeArmStretchAmount ArmFull => Player.CompositeArmStretchAmount.Full;

        public static Player.CompositeArmStretchAmount ArmNone => Player.CompositeArmStretchAmount.None;

        public static Player.CompositeArmStretchAmount ArmQuarter => Player.CompositeArmStretchAmount.Quarter;

        public static Player.CompositeArmStretchAmount ArmThreeQuarters => Player.CompositeArmStretchAmount.ThreeQuarters;
        
        #endregion

        public short Type { get; internal set; }

        protected override void Register()
        {
            ModTypeLookup<CastingAnimation>.Register(this);
            Type = (short)CastingAnimationLoader.ReserveAnimationID();

            SetStaticDefaults();

            CastingAnimationLoader.animations.Add(this);
            Load();
        }

        public virtual void Update(Player player, float progress)
        {

        }
    }
}
