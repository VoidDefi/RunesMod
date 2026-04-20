using RunesMod.Projectiles.Magic.Elements.Fire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;

namespace RunesMod.Systems.Knowledges
{
    public class KnowledgeSystem : ModPlayer
    {
        public List<IKnowledge> Knowledges { get; private set; } = new List<IKnowledge>();

        public void Learning(IKnowledge knowledge)
        {
            if (knowledge != null && knowledge?.CanLearning(Player) == true)
            {
                if (Knowledges.FindIndex(k => k.Compare(knowledge)) < 0)
                {
                    Knowledges.Add(knowledge);
                    knowledge.OnLearn(Player);

                    InGameNotificationsTracker.AddNotification(new KnowledgeNotification(knowledge));
                }
            }
        }

        public override void SaveData(TagCompound tag)
        {
            base.SaveData(tag);
        }

        public override void LoadData(TagCompound tag)
        {
            base.LoadData(tag);
        }
    }
}
