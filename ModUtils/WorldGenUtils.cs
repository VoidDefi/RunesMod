using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.WorldBuilding;

namespace RunesMod.ModUtils
{
    public static class WorldGenUtils
    {
        public static Point GetRandomPosition(int startX, int endX, int startY, int endY, Func<Tile, bool> tileParam)
        {
            int x = WorldGen.genRand.Next(startX, endX);
            int y = WorldGen.genRand.Next(startY, endY);

            int counter = 0;

            while (!tileParam.Invoke(Main.tile[x, y]) && counter < Main.maxTilesX / 5)
            {
                x = WorldGen.genRand.Next(startX, endX);
                y = WorldGen.genRand.Next(startY, endY);
                counter++;
            }

            return new Point(x, y);
        }

        public static void GenOre(float countFactor, Range strengthRange, Range stepRange, int type, Func<Tile, bool> tileParam, int startY = -1, int endY = -1)
        {
            int count = (int)((Main.maxTilesX * countFactor) * 0.2f);

            int endX = Main.maxTilesX;
            int start = startY > -1 ? startY : (int)Main.rockLayer;
            int end = endY > -1 ? endY : Main.maxTilesY - 200;

            for (int i = 0; i < count; i++)
            {
                Point position = WorldGenUtils.GetRandomPosition(0, endX, start, end, tileParam);

                int strength = WorldGen.genRand.Next(strengthRange.Start.Value, strengthRange.End.Value);
                int steps = WorldGen.genRand.Next(stepRange.Start.Value, stepRange.End.Value);

                WorldGen.TileRunner(position.X, position.Y, strength, steps, type);
            }
        }
    }
}
