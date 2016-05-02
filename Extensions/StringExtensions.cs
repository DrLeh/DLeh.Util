using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DLeh.Util.Extensions
{
    public static class StringExtensions
    {
        public static string Format(this string str, params object[] args)
        {
            return string.Format(str, args);
        }
        public static bool EqualsAny(this string str, params string[] args)
        {
            return args.Any(x => x == str);
        }
        public static bool ContainsAny(this string str, params string[] args)
        {
            return args.Any(x => str.Contains(x));
        }


        public static string Abbreviate(this string str, bool preserveSpaces = false)
        {
            var sb = new StringBuilder();
            foreach (var word in str.Split(' ', '-'))
            {
                sb.Append(word.GetFirstLetterAndCapitals());//.Append(".");
                var numberAtEnd = GetNumberAtEndOfString(word);
                if (!string.IsNullOrEmpty(numberAtEnd))
                    sb.Append(numberAtEnd);
                if (preserveSpaces)
                    sb.Append(" ");
            }
            return sb.ToString().ToUpper();
        }
        public static string GetFirstLetterAndCapitals(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return string.Empty;
            var sb = new StringBuilder();
            sb.Append(str[0]);
            if (str.Length > 1)
                for (int i = 1; i < str.Length; i++)
                    if (char.IsUpper(str[i]))
                        sb.Append(str[i]);
            return sb.ToString();
        }
        public static string GetNumberAtEndOfString(this string str)
        {
            return Regex.Match(str, @"\d+$").Value;
        }
        public static string CapitalizeEachFirstCharacter(this string str)
        {
            if (str == null)
                return null;
            if (str.Length < 1)
                return "";
            if (str.Length == 1)
                return str.ToUpper();

            var strings = str.Split(' ');
            var sb = new StringBuilder();
            var firstString = strings[0];
            sb.Append(firstString[0].ToString().ToUpper());
            if (firstString.Length > 1)
                sb.Append(firstString.Substring(1));
            if (strings.Count() > 1)
                foreach (var remainingString in strings.Skip(1))
                    sb.Append(" ").Append(CapitalizeEachFirstCharacter(remainingString));
            return sb.ToString();
        }
    }
}
