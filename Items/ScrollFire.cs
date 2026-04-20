using RunesMod.ModUtils;
using RunesMod.Shields.Elements.Fire;
using RunesMod.Spells;
using RunesMod.Spells.Defense.Elements.Fire;
using RunesMod.Spells.Elements.Fire;
using RunesMod.Systems.Knowledges;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Achievements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace RunesMod.Items
{
    public class ScrollFire : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 8;
            Item.height = 8;
            Item.maxStack = 9999;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.UseSound = SoundID.Item4;
        }

        public override bool? UseItem(Player player)
        {
            SpellKnowledge fireBall = new(SpellLoader.GetSpell<FireBallSpell>());
            player.KnowledgeSystem().Learning(fireBall);

            SpellKnowledge shield = new(SpellLoader.GetSpell<FireShieldSpell>());
            player.KnowledgeSystem().Learning(shield);

            //InGameNotificationsTracker.AddNotification(new KnowledgeNotification(fireBall));

            //InGameNotificationsTracker.AddNotification(new InGamePopups.AchievementUnlockedPopup(Main.Achievements.GetAchievement("GET_A_LIFE")));

            return true;
        }
    }
}
