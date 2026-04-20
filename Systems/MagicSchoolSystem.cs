using RunesMod.DamageClasses;
using RunesMod.MagicSchools;
using RunesMod.ModUtils;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace RunesMod.Systems
{
    public class MagicSchoolSystem : ModPlayer
    {
        private DamageClassData[] boosts = null;

        public DamageClassData[] Boosts
        {
            get
            {
                if (boosts == null)
                {
                    if (MagicSchoolLoader.SchoolCount > 0)
                    {
                        boosts = new DamageClassData[MagicSchoolLoader.SchoolCount];
                    }
                }

                return boosts;
            }
        }
            

        public override void ResetEffects()
        {
            for (int i = 0; i < Boosts.Length; i++)
            {
                MagicSchool school = MagicSchoolLoader.GetSchool(i);

                if (school.OnlyCategory)
                {
                    Boosts[i] = null;
                }
                //Player.GetDamage
                Boosts[i] = school.DefaultDamage;
            }
        }

        public DamageClassData GetDamageData(MagicSchool school)
        {
            if (school?.OnlyCategory == false)
            {
                int type = school.Type;

                if (type > 0 && type < Boosts.Length)
                {
                    return Boosts[type];
                }
            }

            return null;
        }
    }
}
