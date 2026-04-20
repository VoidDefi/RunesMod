using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace RunesMod.Items.Materials
{
    public class VoidFragment : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
            Item.ResearchUnlockCount = 25;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 24;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(0, 0, 5, 0);
            Item.rare = ModContent.GetInstance<Rarities.VoidRarity>().Type;
        }

        public override void PostUpdate()
        {
            Lighting.AddLight(Item.Center, 0.1f, 0.0f, 0.12f);

            if (Main.rand.NextFloat() < 0.04139535f)
            {
                Dust dust;
                Vector2 position = Item.position;
                dust = Main.dust[Dust.NewDust(position, Item.width, Item.height, 132, 0f, -1, 0, new Color(255, 0, 0), 1f)];
                dust.noGravity = true;
                dust.shader = GameShaders.Armor.GetSecondaryShader(93, Main.LocalPlayer);
            }
        }

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            return base.PreDrawInInventory(spriteBatch, position, frame, drawColor, itemColor, origin, scale);
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Texture2D texture = TextureAssets.Item[ModContent.ItemType<VoidFragment>()].Value;
            Vector2 origin = new(texture.Width / 2, texture.Height / 2);

            List<Color> Colors = new List<Color>() { new Color(45, 22, 71), new Color(140, 18, 212) };

            float alphaColor_ = Main.GameUpdateCount % 60 / 60f;
            int indexColor = (int)(Main.GameUpdateCount / 60 % 2);
            Color color = Color.Lerp(Colors[indexColor], Colors[(indexColor + 1) % 2], alphaColor_);
            color.A = 15;

            Vector2 position_ = Vector2.One + new Vector2((float)Math.Sin(Main.GameUpdateCount / 35f));

            Vector2 position = Item.position - Main.screenPosition + new Vector2(Item.width / 2 - 0.5f, Item.height / 2 - 0.5f);

            spriteBatch.Draw(texture, position + position_.RotatedBy(Main.GameUpdateCount / 15f + MathHelper.Pi), null, color, rotation, origin, scale, SpriteEffects.None, 0f);
            spriteBatch.Draw(texture, position + position_.RotatedBy(Main.GameUpdateCount / 15f + MathHelper.PiOver2), null, color, rotation, origin, scale, SpriteEffects.None, 0f);
            spriteBatch.Draw(texture, position + position_.RotatedBy(Main.GameUpdateCount / 15f - MathHelper.PiOver2), null, color, rotation, origin, scale, SpriteEffects.None, 0f);
            spriteBatch.Draw(texture, position + position_.RotatedBy(Main.GameUpdateCount / 15f), null, color, rotation, origin, scale, SpriteEffects.None, 0f);

            return true;
        }
    }
}