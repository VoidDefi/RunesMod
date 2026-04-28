using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using RunesMod.ModUtils;
using RunesMod.ModUtils.Reflection;
using RunesMod.Spells;
using RunesMod.Spells.Defense.Elements.Fire;
using System;
using System.Reflection;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Renderers;
using Terraria.ID;
using Terraria.ModLoader;

namespace RunesMod.NPCs
{
    public abstract class PlayerNPC : ModNPC
    {
        private AutoMethodInfo DrawPlayerFullInfo;

        public Player player;

        public sealed override void OnSpawn(IEntitySource source)
        {
            //Player player = Main.player[Main.myPlayer];
            //player.hair = 15;
            //player.skinColor = Color.White;
            //player.skinVariant = 10;
            //player.shimmering = true;

            player = new Player();
            player.hair = 15;
            //player.skinColor = Color.White;
            //player.skinVariant = 10;
            player.name = "???";
            player.UseHealthMaxIncreasingItem(20 * 15);
            player.ConsumedLifeCrystals = 15;

            player.UseManaMaxIncreasingItem(20 * 10);
            player.ConsumedManaCrystals = 9;

            NPC.width = player.width;
            NPC.height = player.height;

            Item head;
            Item body;
            Item leg;

            while (true)
            {
                head = new Item(Main.rand.Next(1, ItemLoader.ItemCount));
                if (head.headSlot != -1) break;
            }

            while (true)
            {
                body = new Item(Main.rand.Next(1, ItemLoader.ItemCount));
                if (body.bodySlot != -1) break;
            }

            while (true)
            {
                leg = new Item(Main.rand.Next(1, ItemLoader.ItemCount));
                if (leg.legSlot != -1) break;
            }

            player.armor[0] = head;
            player.armor[1] = body;
            player.armor[2] = leg;

            for (int i = 3; i <= 9; i++)
            {
                Item acs;

                while (true)
                {
                    acs = new Item(Main.rand.Next(1, ItemLoader.ItemCount));
                    if (acs.accessory) break;
                }

                player.armor[i] = acs;
            }

            player.Male = Main.rand.NextBool();

            player.hair = Main.rand.Next(0, HairID.Count);
            //player.skinColor = Color.White;
            player.skinVariant = Main.rand.Next(0, PlayerVariantID.Count);

            if (player.skinVariant == 10)
            {
                player.skinVariant = PlayerVariantID.MaleStarter;
            }

            player.active = true;

            player.PlayerStates().TrueMagic = true;
            player.controlUseItem = true;
            player.SpellSystem().TryCastSpell(SpellLoader.GetSpell<FireShieldSpell>());

            NPC.lifeMax = player.statLifeMax;
            NPC.life = player.statLifeMax;

            Spawned(source);
        }

        public sealed override void AI()
        {
            player.direction = NPC.direction;
            player.velocity = NPC.velocity;
        }

        public sealed override void OnKill()
        {
            player.KillMe(new PlayerDeathReason() { SourceCustomReason = "брббрбрбрбрб тфу... выаываываыва" }, 0, 0);
        }

        public sealed override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (player == null) OnSpawn(null);

            player.position = NPC.position;

            player.isDisplayDollOrInanimate = true;
            player.ResetEffects();
            player.ResetVisibleAccessories();
            player.UpdateDyes();
            player.DisplayDollUpdate();
            player.UpdateSocialShadow();
            player.PlayerFrame();

            Tile tile = Framing.GetTileSafely((int)player.position.X / 16, (int)player.position.Y / 16);
            player.isFullbright = tile.IsTileFullbright;
            player.skinDyePacked = PlayerDrawHelper.PackShader(tile.TileColor, PlayerDrawHelper.ShaderConfiguration.TilePaintID);

            if (Main.PlayerRenderer is LegacyPlayerRenderer && false)
            {
                LegacyPlayerRenderer renderer = Main.PlayerRenderer as LegacyPlayerRenderer;

                DrawPlayerFullInfo = new AutoMethodInfo(typeof(LegacyPlayerRenderer), "DrawPlayerFull", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);

                DrawPlayerFullInfo.Value.Invoke(renderer, [Main.Camera, player]);
            }

            else
            {
                Main.Camera.SpriteBatch.End();
                Main.Camera.SpriteBatch.Begin();

                Main.PlayerRenderer.DrawPlayer(Main.Camera, player, player.position, 0f, player.fullRotationOrigin);

                Main.Camera.SpriteBatch.End();
                Main.Camera.SpriteBatch.Begin(null);
            }
        }

        public virtual void Spawned(IEntitySource source)
        {

        }
    }

    public class PlayerNPCSystem : GlobalNPC
    {
        #region On

        public override void Load()
        {
            On_NPC.CalculateHitInfo += OnCalculateHitInfoHook;
        }

        private NPC.HitInfo OnCalculateHitInfoHook(On_NPC.orig_CalculateHitInfo original, NPC npc, int damage, int hitDirection, bool crit, float knockBack, DamageClass damageType, bool damageVariation, float luck)
        {
            if (npc.ModNPC != null && npc.ModNPC.GetType().IsSubclassOf(typeof(PlayerNPC)))
            {
                PlayerNPC playerNPC = npc.ModNPC as PlayerNPC;

                playerNPC.player.Hurt(null, damage, hitDirection, out Player.HurtInfo info, false);

                return npc.GetIncomingStrikeModifiers(damageType, hitDirection).ToHitInfo(info.Damage, crit, info.Knockback, damageVariation, luck);
            }

            return original.Invoke(npc, damage, hitDirection, crit, knockBack, damageType, damageVariation, luck);
        }

        #endregion
    }
}
