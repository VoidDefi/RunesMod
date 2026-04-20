using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace RunesMod.WorldGeneration
{
    public abstract class AutoGenPass : GenPass
    {
        public Mod Mod { get; set; }

        public virtual string InsertPassName { get; set; } = null;

        public virtual string PassName { get; set; } = null;

        public string LocalizationCategory => "GenerationPassages";

        public AutoGenPass() : base("", 100)
        {
            Name = PassName;
        }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {

        }

        public LocalizedText CreatePassName()
        {
            string path = $"Mods.{Mod.Name}.{LocalizationCategory}.{GetType().Name}.Tooltip";
            Func<string> fallback = () => Regex.Replace(PassName ?? GetType().Name,
                                                        "([A-Z])", " $1").Trim();

            return Language.GetOrRegister(path, fallback);
        }
    }
}
