using RunesMod.DamageClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using RunesMod.ModUtils;
using RunesMod.MagicSchools;

namespace RunesMod.Systems
{
    public class MagicSchoolProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        private int[] magicSchools = null;

        public void Add(int type)
        {
            MagicSchool.AddToArray(ref magicSchools, type);
        }

        public void Add<T>() where T : MagicSchool => Add(MagicSchoolLoader.SchoolType<T>());

        public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (modifiers.DamageType is TrueMagic && projectile.TryGetOwner(out Player player))
            {
                if (magicSchools != null && magicSchools.Length > 0)
                {
                    for (int i = 0; i < magicSchools.Length; i++)
                    {
                        MagicSchool school = MagicSchoolLoader.GetSchool(magicSchools[i]);

                        if (school?.OnlyCategory == false)
                        {
                            StatModifier damage = player.MagicSchoolSystem().Boosts[school.Type].damage;
                            modifiers.FinalDamage = modifiers.FinalDamage.CombineWith(damage);
                        }
                    }
                }
            }
        }
    }
}
