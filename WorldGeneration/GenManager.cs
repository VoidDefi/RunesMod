using RunesMod.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.Generation;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.WorldBuilding;
using RunesMod.WorldGeneration.Ores;
using MonoMod.Cil;

namespace RunesMod.WorldGeneration
{
    public class GenManager : ModSystem
    {
        List<AutoGenPass> GenPasses { get; set; } = new List<AutoGenPass>();

        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
        {
            foreach (AutoGenPass autoGenPass in GenPasses)
            {
                if (autoGenPass.PassName != null)
                {
                    if (autoGenPass.InsertPassName != null)
                    {
                        int index = tasks.FindIndex(pass => pass.Name.Equals(autoGenPass.InsertPassName));
                        if (index != -1)
                        {
                            autoGenPass.Name = autoGenPass.Mod.Name + ": " + autoGenPass.Name;
                            tasks.Insert(index + 1, autoGenPass);
                        }
                    }

                    else tasks.Add(autoGenPass);
                }
            }
        }

        public override void Load()
        {
            Type autoGenPassType = typeof(AutoGenPass);
            List<Type> modTypes = Assembly.GetAssembly(autoGenPassType).GetTypes().ToList();
            List<Type> types = modTypes.Where(type => type.IsSubclassOf(autoGenPassType)).ToList();

            foreach (Type type in types)
            {
                if (type.IsAbstract || type.GetConstructor(Type.EmptyTypes) == null)
                    continue;

                AutoGenPass autoGenPass = Activator.CreateInstance(type) as AutoGenPass;
                autoGenPass.Mod = Mod;
                
                GenPasses.Add(autoGenPass);
            }
        }

        public static void ModifyGenPass(string passName, ILContext.Manipulator detour)
        {
            WorldGen.ModifyPass((PassLegacy)WorldGen.VanillaGenPasses[passName], detour);
        }
    }
}
