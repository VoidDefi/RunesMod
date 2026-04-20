using RunesMod.ModUtils;
using RunesMod.Tiles.Ores;
using RunesMod.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using Terraria;

namespace RunesMod.WorldGeneration.Ores
{
    public class GemsGenerator : AutoGenPass
    {
        public override string InsertPassName => "Random Gems";

        public override string PassName => "Gems";

        public GemsGenerator() : base() { }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = CreatePassName().Value;

            for (int i = 50; i < Main.maxTilesX - 50; i++)
            {
                for (int j = (int)Main.rockLayer; j < Main.maxTilesY - 50; j++)
                {
                    if (Main.tile[i, j].HasTile && Main.tile[i, j].TileType == ModContent.TileType<RockCrystalOre>())
                    {
                        for (int k = -1; k < 2; k++)
                        {
                            for (int l = -1; l < 2; l++)
                            {
                                if (k == l) continue;

                                int x = i + k;
                                int y = j + l;

                                if (!Main.tile[x, y].HasTile && WorldGen.genRand.Next(0, 3) == 0)
                                {
                                    int type = ModContent.TileType<RockCrystal>();
                                    if (WorldGen.PlaceTile(x, y, type, true))
                                    {
                                        Framing.GetTileSafely(x, y).TileFrameX = (short)(Main.rand.Next(0, 8) * 18);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
