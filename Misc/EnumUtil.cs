using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace DLeh.Util
{
    public static class EnumUtil
    {
        public static bool IsValidValueFor<T>(this int value) where T : struct
        {
            return RepresentsValidValue<T>(value);
        }

        public static bool RepresentsValidValue<T>(int value) where T : struct
        {
            return EnumUtil.GetValues<T>().Cast<int>().Any(x => x == value);
        }

        public static IEnumerable<T> GetValues<T>()
        {
            if (!typeof(T).IsEnum)
                throw new NotSupportedException($"Type '{typeof(T).Name}' is not supported");

            return Enum.GetValues(typeof(T)).Cast<T>();
        }

        public static string GetDescription(this object enumerationValue)
        {
            Type type = enumerationValue.GetType();
            if (!type.IsEnum)
                throw new ArgumentException("EnumerationValue must be of Enum type", nameof(enumerationValue));

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


        public static bool HasFlag(this Enum variable, Enum value)
        {
            if (variable == null)
                return false;

            if (value == null)
                throw new ArgumentNullException(nameof(value));

            // Not as good as the .NET 4 version of this function, but should be good enough
            if (!Enum.IsDefined(variable.GetType(), value))
            {
                throw new ArgumentException(string.Format(
                    "Enumeration type mismatch.  The flag is of type '{0}', was expecting '{1}'.",
                    value.GetType(), variable.GetType()));
            }

            ulong num = Convert.ToUInt64(value);
            return ((Convert.ToUInt64(variable) & num) == num);
        }


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

        public static bool HasFlagSet(this Enum a, Enum flagValue)
        {
            if (a.GetType() != flagValue.GetType())
                throw new ArgumentException(String.Format("Argument_EnumTypeDoesNotMatch {0} {1}", flagValue.GetType(), a.GetType()));

            ulong bitField = Convert.ToUInt64(a);
            ulong flag = Convert.ToUInt64(flagValue);

            return (flag & bitField) == flag;
        }

        public static IEnumerable<T> GetNonDisabledEnums<T>()
        {
            var enumType = typeof(T);
            return GetNonDisabledEnums(enumType).Cast<T>();
        }
        public static IEnumerable<Enum> GetNonDisabledEnums(Type enumType)
        {
            foreach (var enumerationValue in Enum.GetValues(enumType))
            {
                bool show = true;
                MemberInfo[] memberInfo = enumType.GetMember(enumerationValue.ToString());
                if (memberInfo != null && memberInfo.Length > 0)
                {
                    object[] attrs = memberInfo[0].GetCustomAttributes(typeof(DisabledEnumAttribute), false);

                    if (attrs != null && attrs.Length > 0)
                        if (((DisabledEnumAttribute)attrs[0]).Disabled)
                            show = false;
                }
                if (show)
                    yield return (Enum)enumerationValue;
            }
        }

    }
}
