using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace DLeh.Util
{
    public static class DateTimeExtensions
    {
        public static DateTime MaxOf(DateTime first, DateTime second) => first > second ? first : second;
        public static DateTime MinOf(DateTime first, DateTime second) => first > second ? second : first;

        public static DateTime MoveToNearstFirstOfMonth(this DateTime original)
        {
            if (original.Day == 1)
            {
                return original;
            }
            else
            {
                var newDate = original.AddMonths(1);
                return new DateTime(newDate.Year, newDate.Month, 1);
            }
        }

        public static int GetAgeGivenDOB(DateTime DOB, DateTime ageDate)
        {
            //int now = ageDate.Year * 100000 + ageDate.Month * 1000 + ageDate.Day;
            //int dob = DOB.Year * 100000 + DOB.Month * 1000 + DOB.Day;
            //int diff = now - dob;

            int diff = (ageDate.Year - DOB.Year) * 100000 + (ageDate.Month - DOB.Month) * 1000 + (ageDate.Day - DOB.Day);
            return diff < 9999 ? 0 : diff / 100000;
        }

        public static DateTime FirstDayOfThisMonth(this DateTime dt) => new DateTime(dt.Year, dt.Month, 1);

        public static DateTime FirstDayOfThisQuarter(this DateTime dt)
        {
            var month = dt.Month;
            if (month < 4)
                return new DateTime(dt.Year, 1, 1);
            else if (month < 7)
                return new DateTime(dt.Year, 4, 1);
            else if (month < 10)
                return new DateTime(dt.Year, 7, 1);
            return new DateTime(dt.Year, 10, 1);
        }

        public static DateTime LastDayOfThisMonth(this DateTime dt) => new DateTime(dt.Year, dt.Month, DateTime.DaysInMonth(dt.Year, dt.Month));


        public static string ToShortDateString(this DateTime? dt)
        {
            if (!dt.HasValue)
                return "";
            return dt.Value.ToShortDateString();
        }

        public static string ToYYYYMMDD(this DateTime dt) => dt.ToString("yyyyMMdd");//i always forget it's this format string

        //these will be replacements for the lack of ToShortDateString() in .net core
        public static string ToShortDateString(this DateTime dt) => dt.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);
        public static string ToShortTimeString(this DateTime dt) => dt.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern);

    }
}
