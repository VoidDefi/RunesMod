using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using RunesMod.ModUtils;
using RunesMod.Dusts;
using System;

namespace RunesMod.Tiles.Bricks
{
    public class SulfurBrickTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;

            TileUtils.SetMerges(Type, true, TileUtils.Soils);

            AddMapEntry(new Color(213, 128, 0));
            
            HitSound = SoundID.Tink;
            DustType = ModContent.DustType<SulfurDust>();
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }
    }
}
