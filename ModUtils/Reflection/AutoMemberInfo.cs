using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RunesMod.ModUtils.Reflection
{
    public interface AutoMemberInfo
    {
        public abstract Type Type { get;  set; }

        public abstract string Name { get;  set; }

        public abstract BindingFlags Flags { get; set; }
    }
}
