using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using RunesMod.Spells;
using RunesMod.Spells.Elements.Fire;
using RunesMod.UI.States;
using RunesMod.UI;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader.IO;

namespace RunesMod.Systems.Knowledges
{
    public class SpellKnowledge : IKnowledge
    {
        public Spell Spell { get; set; }

        public LocalizedText DisplayName => Spell.DisplayName;

        public LocalizedText Tooltip => Spell.Tooltip;

        public Asset<Texture2D> Texture => Spell.Texture;

        public SpellKnowledge(Spell spell)
        {
            Spell = spell;
        }

        public void OnLearn(Player player)
        {
            if (player == null || Spell == null) return;

            SpellHotBar hotBar = UIManager.GetUserInterface(SpellHotBar.SpellHotBarInterface).CurrentState as SpellHotBar;
            if (hotBar != null) hotBar.AddSlot(Spell);
        }

        public bool Compare(IKnowledge knowledge)
        {
            if (GetType() != knowledge.GetType()) return false;

            SpellKnowledge container = knowledge as SpellKnowledge;
            return Spell.Type == container.Spell.Type;
        }

        public void SaveData(TagCompound tag)
        {
            if (Spell == null) return;

            tag.Set("type", Spell.Type);
        }

        public void LoadData(TagCompound tag)
        {
            if (!tag.ContainsKey("type")) return;

            Spell = SpellLoader.GetSpell(tag.GetShort("type"));
        }
    }
}
