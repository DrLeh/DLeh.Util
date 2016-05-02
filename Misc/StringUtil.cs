using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Security;
using System.Runtime.InteropServices;
using System.Linq.Expressions;


namespace DLeh.Util
{
    public static class StringUtil
    {
        public static string FormatWith(this string format, params object[] args) => string.Format(format, args);

        public static string IfNotEmptyAdd(this string toAddto, string post, string pre = "")
        {
            return string.IsNullOrEmpty(toAddto) ? "" : pre + toAddto + post;
        }



        /// <summary>
        /// accepts input like "1-2,4,6-9,15", and returns 1,2,4,6,7,8,9,15 as a lis.t The inverse of <see cref="StringifyIntList(string)"/>
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static List<int> GetIntListFromString(string input)
        {
            if (input == null)
                return null;
            if (!input.Any())
                return new List<int>();

            return input.Split(',')
                .Select(x => new
                {
                    Value = x,
                    HasHypen = x.Contains('-'),
                    Splits = x.Split('-')
                })
                .Select(x => new
                {
                    Start = int.Parse(x.HasHypen ? x.Splits[0] : x.Value),
                    End = int.Parse(x.HasHypen ? x.Splits[1] : x.Value),
                })
                .SelectMany(x => Enumerable.Range(x.Start, x.End - x.Start + 1))
                .Distinct()
                .OrderBy(x => x)
                .ToList();
        }

        /// <summary>
        /// Stringifies a list of ints into a string like "1-2,4,6-9,15". The inverse of <see cref="GetIntListFromString(string)"/>
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string StringifyIntList(List<int> input)
        {
            if (input == null)
                return null;
            if (!input.Any())
                return "";

            var sb = new StringBuilder();
            var ordered = input.Distinct().OrderBy(x => x).ToList();

            var last = 0;
            var hypenating = false;
            for (int i = 0; i < ordered.Count(); i++)
            {
                var item = ordered[i];
                if (last == 0)
                    sb.Append(item);
                else if (item - 1 == last)
                {
                    hypenating = true;
                    if (hypenating && i == ordered.Count() - 1)
                        sb.Append("-").Append(item);
                }
                else if (hypenating)
                {
                    hypenating = false;
                    sb.Append("-").Append(last).Append(",").Append(item);
                }
                else
                {
                    sb.Append(",").Append(item);
                }
                last = item;
            }
            return sb.ToString();
        }

        public static List<T> GetListFromString<T>(string input, char separator = ',')
        {
            if (input == null)
                return null;
            if (!input.Any())
                return new List<T>();

            return input.Split(separator)
                .Select(x => (T)Convert.ChangeType(x, typeof(T)))
                .ToList();
        }

        public static List<Tuple<T1, T2>> GetTupleListFromString<T1, T2>(string input, char separator = ';', char innerSeparator = ',')
        {
            if (input == null)
                return null;
            if (!input.Any())
                return new List<Tuple<T1, T2>>();

            return input.Split(separator)
                .Select(x => x.Replace("(", "").Replace(")", "").Split(innerSeparator))
                .Select(x => Tuple.Create((T1)Convert.ChangeType(x[0], typeof(T1)), (T2)Convert.ChangeType(x[1], typeof(T2))))
                .ToList();
        }


        public static string DashPhoneNumber(string number)
        {
            if (number == null)
                return null;

            if (Regex.IsMatch(number, @"(\d{3})-(\d{3})-(\d{4})") || Regex.IsMatch(number, @"(\d)-(\d{3})-(\d{3})-(\d{4})"))
                return number;

            var dashesRemoved = number.Replace("-", "");

            if (dashesRemoved.Length == 10)
                return Regex.Replace(dashesRemoved, @"(\d{3})(\d{3})(\d{4})", "$1-$2-$3");
            if (dashesRemoved.Length == 11)
                return Regex.Replace(dashesRemoved, @"(\d)(\d{3})(\d{3})(\d{4})", "$1-$2-$3-$4");

            return number;
        }
    }
}

