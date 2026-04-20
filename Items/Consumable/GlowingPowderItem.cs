using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RunesMod.ModUtils;
using RunesMod.Projectiles;
using RunesMod.Tiles.CraftStations;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace RunesMod.Items.Consumable
{
    public class GlowingPowderItem : ModItem
    {
        private Vector4 BloomColor => new Vector4(0.08f, 0.3f, 0.82f, 0.3f);

        private Vector4 BloomHotColor => new Vector4(0.08f, 0.3f, 0.82f, 1f);

        private float BloomFactor => 1f;

        public override void SetStaticDefaults() 
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
        }

        public override void SetDefaults()
        {
            Item.damage = 0;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.shootSpeed = 4f;
            Item.shoot = ModContent.ProjectileType<GlowingPowder>();
            Item.width = 16;
            Item.height = 24;
            Item.maxStack = Item.CommonMaxStack;
            Item.consumable = true;
            Item.UseSound = SoundID.Item1;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.noMelee = true;
            Item.value = 100;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.GlowingMushroom, 10);
            recipe.AddTile<Mortar>();
            recipe.Register();
        }

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            DrawBloom(spriteBatch, position, scale, 0f, drawColor);
            return true;
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Vector2 position = Item.position - Main.screenPosition + new Vector2(Item.width / 2, Item.height / 2 - 1f);

            DrawBloom(spriteBatch, position, scale, rotation, alphaColor);
            return true;
        }

        private void DrawBloom(SpriteBatch spriteBatch, Vector2 position, float scale, float rotation, Color drawColor)
        {
            Effect overlapping = ModAssets.Request<Effect>(ModAssets.Effects, "Overlapping").Value;
            Texture2D bloomMask = ModAssets.Request<Texture2D>(ModAssets.MaskTextures, "BagBloomMask").Value;

            float alpha = drawColor.A / 256f;

            overlapping.Parameters["uColor"].SetValue(BloomColor * alpha);
            overlapping.Parameters["uHotColor"].SetValue(BloomHotColor * alpha);
            overlapping.Parameters["uHotFactor"].SetValue(BloomFactor);

            spriteBatch.End();
            spriteBatch.BeginCopy(overlapping);

            Vector2 bloomOrigin = new Vector2(bloomMask.Width * 0.5f, bloomMask.Height * 0.5f);
            Main.EntitySpriteDraw(bloomMask, position, null, Color.White, rotation, bloomOrigin, scale, 0f, 0f);

            spriteBatch.End();
            spriteBatch.BeginCopy();
        }

        private void DrawBloom(SpriteBatch spriteBatch, Vector2 position, float scale, float rotation)
        {
            Effect overlapping = ModAssets.Request<Effect>(ModAssets.Effects, "Overlapping").Value;
            Texture2D bloomMask = ModAssets.Request<Texture2D>(ModAssets.MaskTextures, "BagBloomMask").Value;

            float alpha = 1f - (Item.alpha / 256f);

            overlapping.Parameters["uColor"].SetValue(BloomColor * alpha);
            overlapping.Parameters["uHotColor"].SetValue(BloomHotColor * alpha);
            overlapping.Parameters["uHotFactor"].SetValue(BloomFactor);

            spriteBatch.End();
            spriteBatch.BeginCopy(overlapping);

            Vector2 bloomOrigin = new Vector2(bloomMask.Width * 0.5f, bloomMask.Height * 0.5f);
            Main.EntitySpriteDraw(bloomMask, position, null, Color.White, rotation, bloomOrigin, scale, 0f, 0f);

            spriteBatch.End();
            spriteBatch.BeginCopy();
        }
    }
}
