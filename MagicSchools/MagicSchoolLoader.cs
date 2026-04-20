using System.Collections.Generic;
using Terraria.ModLoader;

namespace RunesMod.MagicSchools
{
    public class MagicSchoolLoader
    {
        private static int nextSchool = 0;

        internal static readonly List<MagicSchool> schools = new List<MagicSchool>();

        public static int SchoolCount => nextSchool;

        internal static int ReserveSchoolID()
        {
            int reserveID = nextSchool;
            nextSchool++;
            return reserveID;
        }

        public static MagicSchool GetSchool(int type)
        {
            if (type < 0 || type >= SchoolCount)
                return null;

            return schools[type];
        }

        public static MagicSchool GetSchool<T>() where T : MagicSchool
        {
            return GetSchool(SchoolType<T>());
        }

        public static int SchoolType<T>() where T : MagicSchool
        {
            return ModContent.GetInstance<T>()?.Type ?? -1;
        }
    }
}
