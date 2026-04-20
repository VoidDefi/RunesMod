using RunesMod.Systems;
using RunesMod.Systems.Knowledges;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using static System.Net.Mime.MediaTypeNames;

namespace RunesMod.ModUtils
{
    public static class PlayerUtils
    {
        #region ModPlayers

        public static SpellSystem SpellSystem(this Player player)
        {
            return player.GetModPlayer<SpellSystem>();
        }

        public static ShieldSystem ShieldSystem(this Player player)
        {
            return player.GetModPlayer<ShieldSystem>();
        }

        public static PlayerStates PlayerStates(this Player player)
        {
            return player.GetModPlayer<PlayerStates>();
        }

        public static MagicSchoolSystem MagicSchoolSystem(this Player player)
        {
            return player.GetModPlayer<MagicSchoolSystem>();
        }

        public static CastingAnimationSystem CastingAnimationSystem(this Player player)
        {
            return player.GetModPlayer<CastingAnimationSystem>();
        }

        public static KnowledgeSystem KnowledgeSystem(this Player player)
        {
            return player.GetModPlayer<KnowledgeSystem>();
        }

        #endregion

        public static bool ConsumeMana(this Player player, int amount, bool pay = true, bool blockQuickMana = false)
        {
            if (player.CheckMana(amount, pay, blockQuickMana))
            {
                player.manaRegenDelay = player.maxRegenDelay;
                return true;
            }

            return false;
        }
    }
}
