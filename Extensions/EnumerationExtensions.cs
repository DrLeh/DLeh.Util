using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DLeh.Util.Extensions
{
    /// <summary>
    /// Contains extension methods for Enum and enums
    /// </summary>
    public static class EnumerationExtensions
    {
        /// <summary>
        /// Applies | logic to Enum type
        /// </summary>
        public static Enum Or(this Enum a, Enum b)
        {
            if (Enum.GetUnderlyingType(a.GetType()) != typeof(ulong))
                return (Enum)Enum.ToObject(a.GetType(), Convert.ToInt64(a) | Convert.ToInt64(b));
            else
                return (Enum)Enum.ToObject(a.GetType(), Convert.ToUInt64(a) | Convert.ToUInt64(b));
        }

        /// <summary>
        /// Applies & logic to Enum type
        /// </summary>
        public static Enum And(this Enum a, Enum b)
        {
            if (Enum.GetUnderlyingType(a.GetType()) != typeof(ulong))
                return (Enum)Enum.ToObject(a.GetType(), Convert.ToInt64(a) & Convert.ToInt64(b));
            else
                return (Enum)Enum.ToObject(a.GetType(), Convert.ToUInt64(a) & Convert.ToUInt64(b));
        }

        /// <summary>
        /// Applies ~ logic to Enum type
        /// </summary>
        public static Enum Not(this Enum a)
        {
            return (Enum)Enum.ToObject(a.GetType(), ~Convert.ToInt64(a));
        }

        //Returns the value in the DescriptionAttribute for the enum
        public static string GetDescription(this object enumerationValue)
        // where T : struct
        {
            Type type = enumerationValue.GetType();
            if (!type.IsEnum)
            {
                throw new ArgumentException("EnumerationValue must be of Enum type", "enumerationValue");
            }

            //Tries to find a DescriptionAttribute for a potential friendly name
            //for the enum
            MemberInfo[] memberInfo = type.GetMember(enumerationValue.ToString());
            if (memberInfo != null && memberInfo.Length > 0)
            {
                object[] attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs != null && attrs.Length > 0)
                {
                    //Pull out the description value
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }
            //If we have no description attribute, just return the ToString of the enum
            return enumerationValue.ToString();

        }
    }
}
