using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace RunesMod.NPCs
{
    public class ABunchOfLife : ModNPC
	{
		public override void SetStaticDefaults()
		{
			var drawNPC = new NPCID.Sets.NPCBestiaryDrawModifiers(0) 
			{
				CustomTexturePath = "RunesMod/Assets/Textures/Bestiary/ABunchOfLife",
				Position = new Vector2(0f, 0f) 
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, drawNPC);
		}

		public override void SetDefaults()
		{
			NPC.width = 16;
			NPC.height = 16;
			NPC.aiStyle = 14;
			AIType = NPCID.JungleBat;
			NPC.damage = 10;
			NPC.defense = 5;
			NPC.lifeMax =  30;
			NPC.knockBackResist = 0f;
			NPC.value = Item.buyPrice(0, 0, 1, 0);
			NPC.noGravity = true;
			NPC.noTileCollide = false;
			NPC.HitSound = SoundID.NPCHit35;
			NPC.DeathSound = SoundID.NPCDeath39;
			//Banner = NPC.type;
			//BannerItem = ModContent.ItemType<ABunchOfLifeBanner>();
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return SpawnCondition.Cavern.Chance * 0.6f;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[2]
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns,
				new FlavorTextBestiaryInfoElement("Иногда жизненная энергия мира образует одухотварённые згустки, которые нападают на всё что двигается и высасуют жизнь при касании, если у них нехватает её")
			});
		}

        public override void AI()
		{
			Dust dust;
			dust = Dust.NewDustPerfect(NPC.Center, 267, new Vector2(0f, 0f), 0, new Color(255, 0, 0), 1.45f);
			dust.noGravity = true;
		}

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
			if (hurtInfo.Damage > 0)
            {
				int Heal = hurtInfo.Damage / 2;
				
				NPC.HealEffect(Heal);
				NPC.life += Heal;

                if (NPC.life > NPC.lifeMax)
					NPC.life = NPC.lifeMax;
            }		
        }

        public override void OnKill()
        {
            base.OnKill();
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.LifeMatter>(), 1, 1, 3));
        }
	}
}