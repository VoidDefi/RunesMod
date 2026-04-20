using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria;

namespace RunesMod.Spells
{
    public interface IEntitySource_WithStatsFromSpell
    {
        /// <summary>
        /// The Player using the Spell. Equal to <see cref="EntitySource_Parent.Entity"/>
        /// </summary>
        public Player Player { get; }

        /// <summary>
        /// The spell being used
        /// </summary>
        public Spell Spell { get; }
    }

    public class EntitySource_SpellCast : EntitySource_Parent, IEntitySource_WithStatsFromSpell
    {
        public Player Player => (Player)Entity;

        public Spell Spell { get; }

        public EntitySource_SpellCast(Player player, Spell spell, string? context = null) : base(player, context)
        {
            Spell = spell;
        }
    }
}
