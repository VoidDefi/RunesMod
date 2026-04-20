using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace RunesMod.Rarities
{
    public class VoidRarity : ModRarity
    {
        public override Color RarityColor => ColorRare();

        public Color ColorRare()
        {
            List<Color> RarityColors = new() { new Color(60, 4, 107), new Color(99, 13, 128) };
            float alpha = Main.GameUpdateCount % 60 / 60f;
            int indexColor = (int)(Main.GameUpdateCount / 60 % 2);
            return Color.Lerp(RarityColors[indexColor], RarityColors[(indexColor + 1) % 2], alpha);
        }

        public override int GetPrefixedRarity(int offset, float valueMult)
        {
            return Type;
        }
    }
}
