using Mono.Cecil.Cil;
using MonoMod.Cil;
using RunesMod.Items;
using RunesMod.Items.Consumable.LootBoxes;
using RunesMod.Items.SlotProtectors;
using RunesMod.ModUtils;
using RunesMod.ModUtils.Reflection;
using System;
using System.Reflection;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RunesMod.WorldGeneration.Other
{
    public class ChestItemsReplacer : ModSystem
    {
        /*
        private static AutoMethodInfo ItemSetDefaultsInfo = new(typeof(Item), "SetDefaults", BindingFlags.Public | BindingFlags.Instance, [typeof(int)]);

        public override void Load()
        {
            IL_WorldGen.AddBuriedChest_int_int_int_bool_int_bool_ushort += ILAddBuriedChestHook;
        }

        private static void ILAddBuriedChestHook(ILContext context)
        {
            try
            {
                ReplaceItem<BrokenMagicMirror>(context, 50);
                ReplaceItem<BrokenMagicMirror>(context, 930);
                ReplaceItem<BrokenIceMirror>(context, 3199);
            }

            catch 
            {
                context.DumpIL();
            }
        }

        #region ReplaceItem Methods

        private static void ReplaceItem<NewItem>(ILContext context, int oldType) where NewItem : ModItem
        {
            ReplaceItem(context, oldType, ModContent.ItemType<NewItem>);
        }

        private static void ReplaceItem(ILContext context, int oldType, Func<int> newType)
        {
            ILCursor c = new ILCursor(context);

            for (int i = 0; i < 5000; i++)
            {
                if (!c.TryGotoNext(i => i.MatchCallvirt(ItemSetDefaultsInfo.Value)))
                    break;

                c.Index--;

                Instruction ins = c.GetInstruction();

                if (ins.Operand is int)
                {
                    if ((int)ins.Operand == oldType)
                    {
                        c.Remove();
                        c.EmitDelegate(newType);
                        break;
                    }
                }

                c.Index += 2;
            }
        }

        private static void ReplaceItem(ILContext context, int oldType, int newType)
        {
            ILCursor c = new ILCursor(context);

            for (int i = 0; i < 1000; i++)
            {
                c.GotoNext(i => i.Match(OpCodes.Callvirt, ItemSetDefaultsInfo.Value));
                c.Index--;

                if (c.Match(OpCodes.Ldc_I4_S, oldType))
                {
                    c.Remove();
                    c.Emit(OpCodes.Ldc_I4_S, newType);
                    break;
                }

                c.Index++;
            }
        }

        #endregion
        */

        public override void PostWorldGen()
        {
            foreach (Chest chest in Main.chest)
            {
                if (chest == null) continue;

                Tile tile = Framing.GetTileSafely(chest.x, chest.y);

                int endSlot = -1;

                for (int i = 0; i < chest.item.Length; i++)
                {
                    Item item = chest.item[i];

                    if (item.type == ItemID.MagicMirror && WorldGen.genRand.Next(11) >= 2)
                        item.SetDefaults(ModContent.ItemType<BrokenMagicMirror>());

                    if (item.type == ItemID.IceMirror && WorldGen.genRand.Next(11) >= 2)
                        item.SetDefaults(ModContent.ItemType<BrokenMagicMirror>());

                    if (item.IsAir && endSlot < 0)
                    {
                        endSlot = i;
                    }
                }

                if (endSlot >= 0)
                {
                    int desert = 10;
                    int golden = 1;
                    int frozen = 11;
                    int water = 17;

                    Item item = chest.item[endSlot];

                    if (tile.TileType == TileID.Containers)
                    {
                        int style = tile.TileFrameX / (18 * 2);

                        if (style == golden)
                        {
                            bool rand = WorldGen.genRand.NextBool();

                            if (rand)
                            {
                                item.SetDefaults(ModContent.ItemType<ScrollEarth>());
                            }

                            else
                            {
                                item.SetDefaults(ModContent.ItemType<ScrollBlood>());
                                chest.item[Math.Clamp(endSlot + 1, 0, chest.item.Length - 1)].SetDefaults(ModContent.ItemType<BloodyPin>());
                            }
                        }

                        else if (style == frozen)
                        {
                            item.SetDefaults(ModContent.ItemType<ScrollFrostyBreath>());
                        }

                        else if (style == water)
                        {
                            item.SetDefaults(ModContent.ItemType<ScrollWaterSplit>());
                        }
                    }

                    if (tile.TileType == TileID.Containers2)
                    {
                        int style = tile.TileFrameX / (18 * 2);

                        if (style == desert)
                        {
                            item.SetDefaults(ModContent.ItemType<ScrollLighting>());
                        }
                    }
                }
            }
        }
    }
}
