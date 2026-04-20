using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;

namespace RunesMod.NPCs
{
    public class TNP : PlayerNPC
    {
        public override void SetDefaults()
        {
            NPC.lifeMax = 30;
            NPC.aiStyle = NPCAIStyleID.Passive;
            AIType = NPCID.Guide;
        }
    }
}
