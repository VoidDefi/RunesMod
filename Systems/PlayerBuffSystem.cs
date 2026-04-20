using MonoMod.Cil;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using RunesMod.Buffs;
using Mono.Cecil.Cil;
using Mono.Cecil;

namespace RunesMod.Systems
{
    public class PlayerBuffSystem : ModPlayer
    {
        public override void Load()
        {
            IL_Player.ApplyPotionDelay += IL_ApplyPotionDelayHook;
            base.Load();
        }

        #region IL_Editing

        private static void IL_ApplyPotionDelayHook(ILContext context)
        {
            bool isSuccess = false;

            BindingFlags binding = BindingFlags.Static | BindingFlags.NonPublic;
            MethodInfo getBuffDelayInfo = typeof(PlayerBuffSystem).GetMethod(nameof(GetBuffDelay), binding);
            //Запускаем трай, чтобы отловить сбой, который тут вызвать как два пальца об асфальт
            try
            {
                //курсор, который указывает на инструкцию
                var c = new ILCursor(context);

                //вечный цикл, чтобы впихнуть изменения перед всеми вызовами AddBuff()
                while (true)
                {
                    //Ищем ldc.i4.s (в этом методе, она встречается только при вызове AddBuff())
                    c.GotoNext(i => i.Match(Mono.Cecil.Cil.OpCodes.Ldc_I4_S));

                    /*
                     * Двигаемся назад, чтобы попасть между:
                     * potionDelay = [value];
                     * AddBuff(21, potionDelay);
                    */
                    c.Index--;

                    /*
                     * Пушим туда две ldarg.0 - первый аргумент метода
                     * (Тут это this (this.ApplyPotionDelay(item)), он считается за аргумент, хоть и не в скобках)
                     * Одна инструкция отвечает за (ldfld System.Int32 Terraria.Player::potionDelay).
                     * Другая, за (stfld System.Int32 Terraria.Player::potionDelay).
                    */
                    c.Emit(Mono.Cecil.Cil.OpCodes.Ldarg_0);
                    c.Emit(Mono.Cecil.Cil.OpCodes.Ldarg_0);

                    //Пушим значение переменной this.potionDelay
                    c.Emit(Mono.Cecil.Cil.OpCodes.Ldfld, typeof(Player).GetField("potionDelay"));

                    //Пушим ldarg.1 - второй аргумент метода: sItem 
                    c.Emit(Mono.Cecil.Cil.OpCodes.Ldarg_1);

                    //Пушим call метода PlayerBuffSystem.GetBuffDelay();
                    c.Emit(Mono.Cecil.Cil.OpCodes.Call, getBuffDelayInfo);

                    //Пушим stfld, который перезаписывает potionDelay
                    c.Emit(Mono.Cecil.Cil.OpCodes.Stfld, typeof(Player).GetField("potionDelay"));

                    /*
                     * Возращяемся обратно, так как мы уже за пушили несколько инструкций и без
                     * этого случится вечный цикл пушинга в одно и тоже место кода
                    */
                    c.GotoNext(i => i.Match(Mono.Cecil.Cil.OpCodes.Ldc_I4_S));

                    isSuccess = true;
                }
            }

            catch (Exception ex)
            {
                /*
                 * При ошибки сохраняем IL Дамп. Но, из-за вечного цикла мы влюбом случае получем исключение
                 * (если GotoNext не найдёт нужное, он его выдаст), поэтому тут есть проверка на успешный пуш.
                */
                if (!isSuccess)
                    MonoModHooks.DumpIL(ModContent.GetInstance<RunesMod>(), context);
            }
        }

        private static int GetBuffDelay(int potionDelay, Item sItem)
        {
            PotionQuality quality = sItem.GetGlobalItem<PotionQualitySystem>().Quality;
            return PotionQualitySystem.GetEditingBuffTime(potionDelay, quality, false);
        }
        
        #endregion

        public static Color? GetObsidianSkinColor(Player player)
        {
            if (player.lavaImmune)
                return new Color(57, 47, 118);//.MultiplyRGB();

            return null;
        }

        public static Color? GetIronSkinColor(Player player)
        {
            if (player.FindBuffIndex(BuffID.Ironskin) > -1)
                return new Color(96, 82, 82);

            return null;
        }

        public static Color? GetNauseaColor(Player player)
        {
            if (player.FindBuffIndex(ModContent.BuffType<Nausea>()) > -1)
                return new Color(112, 196, 56);

            return null;
        }
    }
}
