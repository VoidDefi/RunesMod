using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunesMod.UI
{
    public struct InterfaceData
    {
        public string Name { get; set; } = null;

        public string InsertName { get; set; } = null;

        public string InsertMod { get; set; } = "Vanilla";

        public InterfaceData(string name, string insertTo, string insertMod = "Vanilla") 
        {
            Name = name;
            InsertName = insertTo;
            InsertMod = insertMod;
        }
    }
}
