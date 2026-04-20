using RunesMod.Tiles.CraftStations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace RunesMod.TileEntities
{
    public class SimpleAlchemicalTableEntity : ModTileEntity
    {
        public Item UsedItem { get; set; } = new Item() { active = false };

        public override bool IsTileValidForEntity(int x, int y)
        {
            Tile tile = Main.tile[x, y];
            return tile.HasTile && tile.TileType == ModContent.TileType<SimpleAlchemicalTable>();
        }

        public override void SaveData(TagCompound tag)
        {
            tag.Add("UsedItem", UsedItem.SerializeData());
        }

        public override void LoadData(TagCompound tag)
        {
            UsedItem = ItemIO.Load(tag.GetCompound("UsedItem"));
        }

        public override void NetSend(BinaryWriter writer)
        {
            UsedItem.Serialize(writer, ItemSerializationContext.Syncing);
        }

        public override void NetReceive(BinaryReader reader)
        {
            UsedItem.DeserializeFrom(reader, ItemSerializationContext.Syncing);
        }

        public override void Update()
        {
        }
    }
}
