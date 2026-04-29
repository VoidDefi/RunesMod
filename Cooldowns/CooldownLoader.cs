using System.Collections.Generic;
using Terraria.ModLoader;

namespace RunesMod.Cooldowns
{
    public class CooldownLoader
    {
        private static int nextCooldown = 0;

        private static readonly List<Cooldown> cooldowns = new List<Cooldown>();

        public static int CooldownCount => nextCooldown;

        internal static int ReserveCooldownID()
        {
            int reserveID = nextCooldown;
            nextCooldown++;
            return reserveID;
        }

        /*
        public static Cooldown GetCooldown(int type)
        {
            if (type < 0 || type >= CooldownCount)
                return null;

            return cooldowns[type];
        }
        */

        /*
        public static Cooldown GetCooldown<T>() where T : Cooldown
        {
            return GetCooldown(CooldownType<T>());
        }
        */

        public static int CooldownType<T>() where T : Cooldown
        {
            return ModContent.GetInstance<T>()?.Type ?? -1;
        }

        internal static void Add(Cooldown cooldown)
        {
            cooldowns.Add(cooldown);
        }
    }
}
