using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using RunesMod.ModUtils;
using RunesMod.Dusts;
using System;

namespace RunesMod.Tiles.Ores
{
    public class Cinnabar : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileOreFinderPriority[Type] = 350;
            Main.tileSpelunker[Type] = true;
            Main.tileShine[Type] = 200;

            TileUtils.SetMerges(Type, true, TileUtils.Soils);

            TileID.Sets.ChecksForMerge[Type] = true;
            TileID.Sets.Ore[Type] = true;

            AddMapEntry(new Color(255, 0, 51), CreateMapEntryName());
            
            MineResist = 1f;
            MinPick = 55;
            HitSound = SoundID.Tink;
            DustType = ModContent.DustType<CinnabarDust>();
        }

        public override void ModifyFrameMerge(int i, int j, ref int up, ref int down, ref int left, ref int right, ref int upLeft, ref int upRight, ref int downLeft, ref int downRight)
        {
            //WorldGen.TileMergeAttemptFrametest(i, j, Type, TileID.Sets.Ash, ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);

            /*int[] tiles = { up, down, left, right };
            bool merge = true;

            for (int k = 0; k < tiles.Length; k++)
            {
                if (tiles[k] != TileID.Ash)
                    merge = false;
            }

            merge |= up != left || up != right || down != left || down != right;

            Main.tileMerge[TileID.Ash][Type] = merge;*/

            //Main.tileMerge[TileID.Ash][Type] = true;


            WorldGen.TileMergeAttempt(-2, TileID.Ash, ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);
        }

        public override bool CanExplode(int i, int j)
        {
            return true;
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }
    }
}
