using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using RunesMod.ModUtils;
using RunesMod.Dusts;

namespace RunesMod.Tiles.Ores
{
    public class Sulfur : ModTile
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

            AddMapEntry(new Color(251, 181, 0), CreateMapEntryName());

            MineResist = 1f;
            MinPick = 55;
            HitSound = SoundID.Tink;
            DustType = ModContent.DustType<SulfurDust>();
        }

        public override void ModifyFrameMerge(int i, int j, ref int up, ref int down, ref int left, ref int right, ref int upLeft, ref int upRight, ref int downLeft, ref int downRight)
        {
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
