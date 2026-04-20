using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria;
using Terraria.Enums;
using Microsoft.Xna.Framework;
using RunesMod.Items.Placeables;
using RunesMod.Dusts;
using RunesMod.ModUtils;

namespace RunesMod.Tiles.Ores
{
    public class RockCrystalOre : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileOreFinderPriority[Type] = 320;
            Main.tileSpelunker[Type] = true;
            Main.tileShine2[Type] = true;
            Main.tileShine[Type] = 200;

            TileUtils.SetMerges(Type, true, TileUtils.Soils);
            TileUtils.SetMerges(Type, true, TileUtils.Stones);

            TileID.Sets.ChecksForMerge[Type] = true;
            TileID.Sets.Ore[Type] = true;

            AddMapEntry(new Color(47, 130, 255), CreateMapEntryName());

            MineResist = 1f;
            MinPick = 0;
            HitSound = SoundID.Item27;
            DustType = ModContent.DustType<RockCrystalDust>();
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }

        public override IEnumerable<Item> GetItemDrops(int i, int j)
        {
            List<Item> items = new List<Item>()
            {
                new Item(ModContent.ItemType<RockCrystalItem>(), 2)
            };

            return items;
        }
    }
}
