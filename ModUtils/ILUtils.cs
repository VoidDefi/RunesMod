using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace RunesMod.ModUtils
{
    public static class ILUtils
    {
        internal static void DumpIL(this ILContext context)
        {
            MonoModHooks.DumpIL(ModContent.GetInstance<RunesMod>(), context);
        }

        public static Instruction GetInstruction(this ILCursor cursor)
        {
            return cursor.Instrs[cursor.Index];
        }

        public static bool Match<T>(this ILCursor cursor, OpCode opCode, T value)
        {
            return cursor.GetInstruction().Match(opCode, value);
        }
    }
}
