using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace RunesMod.Systems.Alchemy
{
    public class AlchemyRecipe
    {
        public bool UseWater { get; private set; } = true;

        public short UsePotionType { get; private set; } = 0;

        public List<short> Ingredients { get; private set; } = new List<short>();

        public void AddIngredient(int type, int count)
        {

        }
    }
}
