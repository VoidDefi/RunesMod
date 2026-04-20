using RunesMod.ModUtils;
using RunesMod.Spells;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;

namespace RunesMod.Systems.Crafting
{
    public static class StatConsume
    {
        public static Recipe AddStatConsume(this Recipe recipe, CostTypes costType, int amount)
        {
            string costTypeName = costType.ToString();

            string name = Language.GetTextValue($"Mods.RunesMod.Conditions.{costTypeName}1");

            if (amount % 10 == 1 && amount != 11)
                name = Language.GetTextValue($"Mods.RunesMod.Conditions.{costTypeName}");

            recipe.AddCondition(new Condition(amount + " " + name, () => Main.player[Main.myPlayer].statMana >= amount));

            switch (costType)
            {
                case CostTypes.Mana:
                    recipe.AddOnCraftCallback((recipe, item, items, stack) =>
                        Main.player[Main.myPlayer].ConsumeMana(amount));
                    break;
            }

            return recipe;
        }
    }
}
