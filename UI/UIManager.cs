using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using RunesMod.UI.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace RunesMod.UI
{
    [Autoload(Side = ModSide.Client)]
    public class UIManager : ModSystem
    {
        private static Dictionary<InterfaceData, UserInterface> Interfaces { get; set; } = new Dictionary<InterfaceData, UserInterface>();

        internal static List<AutoUIState> UIStates { get; set; } = new List<AutoUIState>();

        public override void Load()
        {
            Interfaces = new Dictionary<InterfaceData, UserInterface>();
            UIStates = new List<AutoUIState>();

            Type autoUIStateType = typeof(AutoUIState);
            List<Type> modTypes = Assembly.GetAssembly(autoUIStateType).GetTypes().ToList();
            List<Type> types = modTypes.Where(type => type.IsSubclassOf(autoUIStateType)).ToList();

            foreach (Type type in types)
            {
                if (type.IsAbstract) continue;

                AutoUIState autoUIState = Activator.CreateInstance(type) as AutoUIState;

                if (!autoUIState.UserInterface.HasValue) continue;

                if (!Interfaces.ContainsKey(autoUIState.UserInterface.Value))
                    Interfaces.Add(autoUIState.UserInterface.Value, new UserInterface());

                Interfaces[autoUIState.UserInterface.Value].SetState(autoUIState);

                UIStates.Add(autoUIState);
            }
        }

        public override void UpdateUI(GameTime gameTime)
        {
            foreach (var ui in Interfaces)
            {
                ui.Value.Update(gameTime);
            }
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            foreach (var ui in Interfaces)
            {
                string insertName = ui.Key.InsertMod + ": " + ui.Key.InsertName;
                int index = layers.FindIndex(layer => layer.Name.Equals(insertName));

                if (index != -1)
                {
                    layers.Insert(index, new LegacyGameInterfaceLayer(
                        Mod.Name + ":" + ui.Key.Name, 
                        delegate 
                        {
                            ui.Value.Draw(Main.spriteBatch, new GameTime()); 
                            return true; 
                        },
                        InterfaceScaleType.UI)
                    );
                }
            }
        }

        public static UserInterface GetUserInterface(string name)
        {
            int index = Interfaces.ToList().FindIndex(ui => ui.Key.Name == name);
            if (index > 0) return Interfaces.ElementAt(index).Value;

            return null;
        }

        public static UserInterface GetUserInterface(InterfaceData data)
        {
            if (Interfaces.TryGetValue(data, out UserInterface ui)) return ui;

            return null;
        }
    }
}
