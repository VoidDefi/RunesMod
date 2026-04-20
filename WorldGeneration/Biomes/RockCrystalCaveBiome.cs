using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace RunesMod.WorldGeneration.Biomes
{
    [Autoload(false)]
    public class RockCrystalCaveBiome : ModBiome
    {
        public override int Music => -1;

        public override SceneEffectPriority Priority => SceneEffectPriority.BiomeLow;

        public override string BestiaryIcon => base.BestiaryIcon;

        public override string BackgroundPath => base.BackgroundPath;

        public override Color? BackgroundColor => base.BackgroundColor;

        public override bool IsBiomeActive(Player player)
        {
            return player.ZoneRockLayerHeight &&
                   Main.tile[player.position.ToTileCoordinates()].WallType ==
                   ModContent.WallType<ModWall>();
        }
    }
}