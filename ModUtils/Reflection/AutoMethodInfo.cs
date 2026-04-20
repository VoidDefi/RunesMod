using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RunesMod.ModUtils.Reflection
{
    public class AutoMethodInfo : AutoMemberInfo
    {
        private MethodInfo value;

        public Type Type { get; set; }
        
        public string Name { get; set; }
        
        public BindingFlags Flags { get; set; }

        public Type[] Arguments { get; set; }

        public MethodInfo Value
        {
            get
            {
                if (value == null)
                {
                    if (Arguments != null)
                        value = Type.GetMethod(Name, Flags, Arguments);

                    else 
                        value = Type.GetMethod(Name, Flags);

                    if (value == null) throw new Exception("Not found method!");
                }

                return value;
            }
        }

        public AutoMethodInfo(Type type, string name, BindingFlags flags, Type[] arguments = null)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("name");

            Type = type;
            Name = name;
            Flags = flags;
            Arguments = arguments;
        }
    }
}
