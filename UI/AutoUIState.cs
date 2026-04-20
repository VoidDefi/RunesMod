using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.UI;

namespace RunesMod.UI
{
    public abstract class AutoUIState : UIState
    {
        public virtual InterfaceData? UserInterface => null;
    }
}
