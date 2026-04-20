using Terraria.DataStructures;
using Terraria.Localization;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.Enums;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace RunesMod.Tiles.CraftStations
{
    public class Mortar : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;

            TileObjectData.newTile.Width = 1;
            TileObjectData.newTile.Height = 1;
            TileObjectData.newTile.CoordinateHeights = new int[] { 16 };
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.AnchorBottom = new AnchorData   
            (
                AnchorType.SolidSide |
                AnchorType.SolidTile | 
                AnchorType.Table | 
                AnchorType.SolidWithTop,           
                TileObjectData.newTile.Width, 
                0
            );
            TileObjectData.addTile(Type);

            AddMapEntry(new Color(96, 86, 77), CreateMapEntryName());

            DustType = DustID.Stone;
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }
    }
}
