using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RunesMod.ModUtils;
using RunesMod.ModUtils.Reflection;
using RunesMod.Spells.Defense.Elements.Fire;
using RunesMod.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using System.Reflection;
using Terraria.Graphics.Renderers;

namespace RunesMod.NPCs
{
    public class Ghost : ModNPC
    {
        private static AutoMethodInfo DrawPlayerFullInfo = new AutoMethodInfo(typeof(LegacyPlayerRenderer), "DrawPlayerFull", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);

        private static AutoAsset<Effect> SpiritEffect = new AutoAsset<Effect>(ModAssets.Effects, "Spirit");

        public Player player;

        public override void SetStaticDefaults()
        {

        }

        public override void SetDefaults()
        {
            NPC.lifeMax = 30;
            NPC.aiStyle = NPCAIStyleID.HoveringFighter; //NPCAIStyleID.Passive;
            AIType = NPCID.Ghost;
            
            NPC.CloneDefaults(NPCID.Ghost);
        }

        public override void OnSpawn(IEntitySource source)
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
            /*
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
            }*/

            head = new Item();
            body = new Item(ItemID.IronChainmail);
            leg = new Item(ItemID.IronGreaves);

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

                //player.armor[i] = acs;
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

            player.GetModPlayer<GhostPlayer>().IsGhost = true;

            NPC.lifeMax = player.statLifeMax;
            NPC.life = player.statLifeMax;
        }

        public override void AI()
        {
            player.direction = NPC.direction;
        }

        #region Hit

        public override bool? CanBeHitByItem(Player player, Item item)
        {
            return base.CanBeHitByItem(player, item);
        }

        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            return base.CanBeHitByProjectile(projectile);
        }

        public override bool CanBeHitByNPC(NPC attacker)
        {
            return base.CanBeHitByNPC(attacker);
        }

        public override void ModifyHitByItem(Player player, Item item, ref NPC.HitModifiers modifiers)
        {
            base.ModifyHitByItem(player, item, ref modifiers);
        }

        public override void ModifyHitByProjectile(Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            base.ModifyHitByProjectile(projectile, ref modifiers);
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            base.ModifyHitNPC(target, ref modifiers);
        }

        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
        {
            base.ModifyHitPlayer(target, ref modifiers);
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            base.HitEffect(hit);
        }

        #endregion

        public override void OnKill()
        {
            base.OnKill();
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            base.ModifyNPCLoot(npcLoot);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
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

                DrawPlayerFullInfo.Value.Invoke(renderer, [Main.Camera, player]);
            }

            else
            {
                DrawingData data = DrawingData.WorldDrawing;
                //data.Effect = SpiritEffect.Value;

                Main.Camera.SpriteBatch.End();
                Main.Camera.SpriteBatch.Begin();
                //data.Begin(Main.Camera.SpriteBatch);

                Main.PlayerRenderer.DrawPlayer(Main.Camera, player, player.position, 0f, player.fullRotationOrigin);

                Main.Camera.SpriteBatch.End();
                Main.Camera.SpriteBatch.Begin(null);
                //DrawingData.WorldDrawing.Begin(Main.Camera.SpriteBatch);
            }

            return false;
        }
    }

    public class GhostPlayer : ModPlayer
    {
        public bool IsGhost { get; set; } = false;

        public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo)
        {
            return;

            if (IsGhost)
            {
                byte alpha = 40;

                Color ghostColor = new Color(106, 211, 255, alpha);

                drawInfo.colorArmorBody = ghostColor;
                drawInfo.colorArmorHead = ghostColor;
                drawInfo.colorArmorLegs = ghostColor;
                drawInfo.colorBodySkin = ghostColor;
                drawInfo.colorDisplayDollSkin = ghostColor;
                drawInfo.colorElectricity = ghostColor;

                Color eyeColor = new Color(0, 30, 80, alpha);
                Color eyeWhiteColor = new Color(217, 244, 255, alpha);

                drawInfo.colorEyes = eyeColor;
                drawInfo.colorEyeWhites = eyeWhiteColor;
                drawInfo.colorHair = ghostColor;
                drawInfo.colorHead = ghostColor;
                drawInfo.colorLegs = ghostColor;
                drawInfo.colorMount = ghostColor;
                drawInfo.colorPants = ghostColor;
                drawInfo.colorShirt = ghostColor;
                drawInfo.colorShoes = ghostColor;
                drawInfo.colorUnderShirt = ghostColor;
            }
        }

        //public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        //{
        //    base.DrawEffects(drawInfo, ref r, ref g, ref b, ref a, ref fullBright);
        //}
    }
}
