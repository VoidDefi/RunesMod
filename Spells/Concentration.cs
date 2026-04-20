using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunesMod.Spells
{
    public class Concentration
    {
        private int maxConcentration = 1;

        public int MaxConcentration
        {
            get => maxConcentration;

            set
            {
                if (value >= 0)
                {
                    maxConcentration = value;
                }
            }
        }

        private List<ConcentrationSlot> Slots { get; set; } = new();

        public float SlotCount => Slots.Count;

        public float SlotLengths => Slots.Sum(s => (int)(s.SlotLength * 2f)) / 2f;

        public ConcentrationSlot GetSlot(int index) => Slots[index];

        public bool HasSlot(Spell spell) => Slots.FindIndex(s => s.SpellType == spell.Type) > -1;

        public void ClearSlots() => Slots.Clear();

        public void SortSlots() => Slots = SortSlots(Slots.ToArray()).ToList();

        public bool AddSlot(Spell spell)
        {
            float length = spell.occupiedSlotLength;

            if (spell.sustainable && length > 0)
            {
                if (SlotLengths + length <= MaxConcentration)
                {
                    if (!HasSlot(spell))
                    {
                        var slot = new ConcentrationSlot(spell, length);

                        Slots.Add(slot);
                        SortSlots();

                        return true;
                    }
                }
            }

            return false;
        }

        public bool RemoveSlot(Spell spell)
        {
            float length = spell.occupiedSlotLength;

            if (spell.sustainable && length > 0)
            {
                if (HasSlot(spell))
                {
                    Slots.RemoveAll(s => s.SpellType == spell.Type);
                    SortSlots();

                    return true;
                }
            }

            return false;
        }

        ConcentrationSlot[] SortSlots(ConcentrationSlot[] array)
        {
            if (array.Length < 2) 
                return array;

            float support = array[0].SlotLength;

            ConcentrationSlot[] left = SortSlots(array.Where(x => x.SlotLength > support).ToArray());
            ConcentrationSlot[] right = SortSlots(array.Where(x => x.SlotLength < support).ToArray());
            ConcentrationSlot[] center = array.Where(x => x.SlotLength == support).ToArray();

            return left.Concat(center).Concat(right).ToArray();
        }
    }
}
