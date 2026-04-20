using Microsoft.Xna.Framework;
using RunesMod.ModUtils;
using RunesMod.Tiles;
using RunesMod.Tiles.Ores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using static tModPorter.ProgressUpdate;

namespace RunesMod.WorldGeneration.Ores
{
    public class GemsBlocksGenerator : AutoGenPass
    {
        public override string InsertPassName => "Gems";

        public override string PassName => "Gems Blocks";

        public GemsBlocksGenerator() : base() { }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = CreatePassName().Value;

            Func<Tile, bool> isStone = (Tile tile) => tile.TileType == TileID.Stone;

            WorldGenUtils.GenOre(0.1f, new(3, 8), new(2, 8), ModContent.TileType<RockCrystalOre>(), isStone);
            WorldGenUtils.GenOre(0.5f, new(2, 6), new(3, 7), ModContent.TileType<RockCrystalInStone>(), isStone);
        }
    }
}
