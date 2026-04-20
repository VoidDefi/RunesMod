using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using RunesMod.ModUtils;
using RunesMod.Dusts;
using System;

namespace RunesMod.Tiles.Bricks
{
    public class LightCinnabarBrickTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;

            TileUtils.SetMerges(Type, true, TileUtils.Soils);

            AddMapEntry(new Color(255, 72, 108));
            
            HitSound = SoundID.Tink;
            DustType = ModContent.DustType<CinnabarDust>();
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }
    }
}
