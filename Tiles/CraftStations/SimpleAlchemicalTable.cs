using Terraria.DataStructures;
using Terraria.Localization;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.Enums;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.GameContent.ObjectInteractions;
using RunesMod.Items.Placeables.CraftStations;
using RunesMod.TileEntities;
using RunesMod.UI.Elements;
using RunesMod.UI.States;

namespace RunesMod.Tiles.CraftStations
{
    public class SimpleAlchemicalTable : ModTile
    {
        public override void SetStaticDefaults()
        {
            TileID.Sets.HasOutlines[Type] = true;
            TileID.Sets.DisableSmartCursor[Type] = true;

            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
            TileObjectData.newTile.Width = 3;
            TileObjectData.newTile.Height = 3;
            TileObjectData.newTile.Origin = new Point16(1, 1);
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 18 };
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.AnchorBottom = new AnchorData   
            (
                AnchorType.SolidSide |
                AnchorType.SolidTile | 
                AnchorType.SolidWithTop,           
                TileObjectData.newTile.Width, 
                0
            );

            TileObjectData.newTile.HookPostPlaceMyPlayer = ModContent.GetInstance<SimpleAlchemicalTableEntity>().Generic_HookPostPlaceMyPlayer;
            TileObjectData.newTile.UsesCustomCanPlace = true;

            TileObjectData.addTile(Type);

            AddMapEntry(new Color(134, 94, 77), CreateMapEntryName());

            DustType = DustID.WoodFurniture;
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }

        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.noThrow = 2;
            player.cursorItemIconEnabled = true;
            player.cursorItemIconID = ModContent.ItemType<SimpleAlchemicalTableItem>();
        }

        public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings)
        {
            return true;
        }

        public override bool RightClick(int i, int j)
        {
            if (TileEntity.TryGet(i, j, out SimpleAlchemicalTableEntity entity))
            {
                if (Main.LocalPlayer.adjTile[ModContent.TileType<SimpleAlchemicalTable>()])
                {
                    SimpleAlchemicalTableSystem.Show(entity);
                }
            }

            return base.RightClick(i, j);
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            ModContent.GetInstance<SimpleAlchemicalTableEntity>().Kill(i, j);
        }
    }
}
