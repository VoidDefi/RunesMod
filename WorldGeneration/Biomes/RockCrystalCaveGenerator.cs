using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace RunesMod.WorldGeneration.Ores
{
    public class RockCrystalCaveGenerator : AutoGenPass
    {
        public override string InsertPassName => "Granite";

        public override string PassName => "Rock Crystal Cave";

        public RockCrystalCaveGenerator() : base() { }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = CreatePassName().Value;

            
        }

        public static void Gem(int x, int y)
        {

        }

        private static bool Gemmable(int type)
        {
            if (type != 0 && type != 1 && type != 40 && type != 59 && type != 60 && type != 70 && type != 147)
            {
                return type == 161;
            }
            return true;
        }
    }
}
