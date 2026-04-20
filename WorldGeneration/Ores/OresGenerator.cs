using RunesMod.ModUtils;
using RunesMod.Tiles.Ores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using Terraria;
using Terraria.ID;
using Terraria.GameContent.Generation;
using Terraria.GameContent.RGB;
using MonoMod.Cil;
using RunesMod.Systems;
using System.Reflection;
using Mono.Cecil.Cil;

namespace RunesMod.WorldGeneration.Ores
{
    public class OresGenerator : AutoGenPass
    {
        public override string InsertPassName => "Gems";

        public override string PassName => "Ores";

        public OresGenerator() : base()
        {
            GenManager.ModifyGenPass("Underworld", ILUnderworldPass);
        }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = CreatePassName().Value;

            Func<Tile, bool> isStone = (Tile tile) => tile.TileType == TileID.Stone;

            WorldGenUtils.GenOre(0.05f, new(2, 4), new(3, 5), ModContent.TileType<Cinnabar>(), isStone, GenVars.lavaLine);
            WorldGenUtils.GenOre(0.05f, new(2, 4), new(3, 5), ModContent.TileType<Sulfur>(), isStone, GenVars.lavaLine);
        }

        #region IL Editing

        public static void ILUnderworldPass(ILContext context)
        {
            BindingFlags binding = BindingFlags.Static | BindingFlags.Public;
            MethodInfo tileRunnerInfo = typeof(WorldGen).GetMethod(nameof(WorldGen.TileRunner), binding);
            
            try
            {
                var c = new ILCursor(context);

                while (c.TryGotoNext(i => i.MatchCall(tileRunnerInfo)))
                {
                    c.Index -= 7;

                    if (c.Next.OpCode == OpCodes.Ldc_I4_S && (sbyte)c.Next.Operand == 58)
                    {
                        if (c.TryGotoNext(i => i.Match(OpCodes.Blt)))
                        {
                            c.Index++;
                            c.EmitDelegate(GenUnderworldOres);
                        }
                    }

                    c.Index += 8;
                }
            }

            catch (Exception ex)
            {
                MonoModHooks.DumpIL(ModContent.GetInstance<RunesMod>(), context);
                throw;
            }
        }
        
        private static void GenUnderworldOres()
        {
            Func<Tile, bool> isAsh = (Tile tile) => tile.TileType == TileID.Ash || tile.TileType == TileID.Hellstone;

            WorldGenUtils.GenOre(0.5f, new(3, 6), new(4, 12), ModContent.TileType<Cinnabar>(), isAsh, Main.maxTilesY - 200, Main.maxTilesY);
            WorldGenUtils.GenOre(0.5f, new(3, 6), new(4, 12), ModContent.TileType<Sulfur>(), isAsh, Main.maxTilesY - 200, Main.maxTilesY);
        }

        #endregion
    }
}
