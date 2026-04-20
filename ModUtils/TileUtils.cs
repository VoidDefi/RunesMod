using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;

namespace RunesMod.ModUtils
{
    public static class TileUtils
    {
        public static int[] Soils => new int[]
        {
            TileID.Dirt,
            TileID.Mud,
            TileID.SnowBlock,
            TileID.ClayBlock,

            TileID.Sand,
            TileID.Ebonsand,
            TileID.Crimsand,
            TileID.Pearlsand,
            
            TileID.Ash,
            TileID.Silt,
            TileID.Slush,
        };

        public static int[] Stones => new int[]
        {
            TileID.Stone,
            TileID.Granite,
            TileID.Marble,
            TileID.Obsidian,

            TileID.Ebonstone,
            TileID.Crimstone,
            TileID.Pearlstone,
            TileID.Pearlstone,

            TileID.Amethyst,
            TileID.Topaz,
            TileID.Sapphire,
            TileID.Emerald,
            TileID.Ruby,
            TileID.Diamond,
            TileID.AmberStoneBlock,

            TileID.ActiveStoneBlock,
            TileID.InactiveStoneBlock
        };

        public static void SetMerges(int tileType, bool isMerge, params int[] merges)
        {
            for (int i = 0; i < merges.Length; i++)
            {
                Main.tileMerge[tileType][merges[i]] = isMerge;
                Main.tileMerge[merges[i]][tileType] = isMerge;
            }
        }
    }
}
