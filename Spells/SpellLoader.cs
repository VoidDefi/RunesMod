using System.Collections.Generic;
using Terraria.ModLoader;

namespace RunesMod.Spells
{
    public class SpellLoader
    {
        private static int nextSpell = 0;

        internal static readonly List<Spell> spells = new List<Spell>();

        public static int SpellCount => nextSpell;

        internal static int ReserveSpellID()
        {
            int reserveID = nextSpell;
            nextSpell++;
            return reserveID;
        }

        public static Spell GetSpell(int type)
        {
            if (type < 0 || type >= SpellCount)
                return null;

            return spells[type];
        }

        public static Spell GetSpell<T>() where T : Spell
        {
            return GetSpell(SpellType<T>());
        }

        public static int SpellType<T>() where T : Spell
        {
            return ModContent.GetInstance<T>()?.Type ?? -1;
        }
    }
}
