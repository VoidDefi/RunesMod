using RunesMod.UI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace RunesMod.UI.States
{
    public class ConcentrationBar : AutoUIState
    {
        public static InterfaceData Interface => new InterfaceData("Concentration Bar", "Mouse Text");

        public override InterfaceData? UserInterface => Interface;

        public ConcentrationBarUI ConcentrationBarUI { get; set; }

        public override void OnInitialize()
        {
            ConcentrationBarUI = new();
            ConcentrationBarUI.VAlign = 0f;
            ConcentrationBarUI.HAlign = 0.5f;
            ConcentrationBarUI.Top.Pixels = 50;
           Append(ConcentrationBarUI);
        }
    }
}
