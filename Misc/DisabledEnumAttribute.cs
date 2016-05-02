using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DLeh.Util
{
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class DisabledEnumAttribute : Attribute
    {
        public DisabledEnumAttribute(bool disabled = true)
        {
            Disabled = disabled;
        }

        public bool Disabled { get; set; }
    }
}
