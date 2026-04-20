using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunesMod.Spells
{
    public struct ConcentrationSlot
    {
        public Spell Spell => SpellLoader.GetSpell(SpellType);

        public int SpellType { get; private set; }

        //public int SlotPosition { get; private set; }

        public float SlotLength { get; private set; }

        public ConcentrationSlot(Spell spell, float slotLength)
        {
            SpellType = spell.Type;
            SlotLength = (float)((int)(slotLength * 2f)) / 2f;
        }
    }
}
