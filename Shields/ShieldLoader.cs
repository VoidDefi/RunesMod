using RunesMod.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace RunesMod.Shields
{
    public class ShieldLoader
    {
        private static int nextShield = 0;

        private static readonly List<(Type shield, short type)> shields = new();

        public static int ShieldCount => nextShield;

        internal static int ReserveShieldID()
        {
            int reserveID = nextShield;
            nextShield++;
            return reserveID;
        }

        public static Shield GetShield(int type)
        {
            if (type < 0 || type >= ShieldCount)
                return null;

            Shield shield = Activator.CreateInstance(shields[type].shield) as Shield;
            shield.Type = shields[type].type;

            return shield;
        }

        public static int ShieldType<T>() where T : Shield
        {
            return ModContent.GetInstance<T>()?.Type ?? -1;
        }

        internal static void AddShield(Type type)
        {
            if (!type.IsSubclassOf(typeof(Shield)))
                throw new ArgumentException();

            short shieldType = (short)ReserveShieldID();

            shields.Add((type, shieldType));
        }
    }
}
