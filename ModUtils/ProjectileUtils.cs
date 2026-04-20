using RunesMod.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace RunesMod.ModUtils
{
    public static class ProjectileUtils
    {
        public static MagicSchoolProjectile MagicSchool(this Projectile projectile)
        {
            return projectile.GetGlobalProjectile<MagicSchoolProjectile>();
        }
    }
}
