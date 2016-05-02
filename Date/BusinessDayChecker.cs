using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

using System.Text;

namespace DLeh.Util
{
    public class BusinessDayChecker
    {
        private BusinessDayChecker()
        {

        }

        public static DateTime NextWeekDay(DateTime dateToCheck)
        {
            DateTime nextDay = dateToCheck.AddDays(1);
            if (nextDay.DayOfWeek == DayOfWeek.Saturday)
                return nextDay.AddDays(2);
            else if (nextDay.DayOfWeek == DayOfWeek.Sunday)
                return nextDay.AddDays(1);
            else
                return nextDay;
        }

        public static DateTime NextNYSETradingDay(DateTime dateToCheck)
        {
            DateTime nextDay = dateToCheck.AddDays(1);

            if (IsNYSEBusinessDay(nextDay))
                return nextDay;

            return NextNYSETradingDay(nextDay);
        }

        public static DateTime PreviousNYSETradingDay(DateTime dateToCheck)
        {
            DateTime prevDate = dateToCheck.AddDays(-1);

            if (IsNYSEBusinessDay(prevDate))
                return prevDate;

            return PreviousNYSETradingDay(prevDate);

        }

        public static bool IsWeekend(DateTime dateToCheck) => ((dateToCheck.DayOfWeek == DayOfWeek.Saturday) || (dateToCheck.DayOfWeek == DayOfWeek.Sunday));

        public static bool IsNYSEBusinessDay(DateTime dateToCheck) => !IsNYSEHolidayOrClosed(dateToCheck);


        private static readonly Func<int, DateTime> _getLastBusinessDay = FuncUtil.MemoizeTimeSensitive<int, DateTime>((year) =>
                                                                                { return GetDateRange(new DateTime(year, 1, 1), new DateTime(year, 12, 31)).Where(x => !IsNYSEHolidayOrClosed(x)).Max(); });
        public static DateTime LastBusinessDayOfYear(int year)
        {
            var lastDayOfYear = new DateTime(year, 12, 31);
            var checkDate = lastDayOfYear;
            while (checkDate.Year == year)
            {
                if (!IsNYSEHolidayOrClosed(checkDate))
                    break;

                checkDate = checkDate.AddDays(-1);
            }

            return checkDate;
        }

        private static readonly Func<int, DateTime> _firstValDateForYear = FuncUtil.MemoizeTimeSensitive<int, DateTime>((year) => { return _getLastBusinessDay(year - 1).AddDays(1); });

        public static DateTime FirstValuationDateForYear(int year) => LastBusinessDayOfYear(year - 1).AddDays(1);

        public static bool IsNYSEHolidayOrClosed(DateTime dateToCheck)
        {
            DateTime checkDate = dateToCheck.Date;

            if (checkDate.DayOfWeek == DayOfWeek.Sunday || checkDate.DayOfWeek == DayOfWeek.Saturday)
                return true;

            return IsNYSEHoliday(checkDate);
        }

        private static bool IsNYSEHoliday(DateTime checkDate)
        {
            // DateTime checkDate = dateToCheck.Date;
            //Debug.Assert(checkDate == checkDate.Date);

            List<DateTime> daysOfYear = GetDateRange(new DateTime(checkDate.Year, 1, 1), new DateTime(checkDate.Year, 12, 31));

            if (checkDate.Month == 1)
            {
                DateTime newyearsDay = NewYearsObserved(checkDate.Year);

                if (newyearsDay == checkDate && newyearsDay != (new DateTime(checkDate.Year - 1, 12, 31)))
                    return true;

                DateTime mlkDay = (from days in daysOfYear
                                   where days.Month == 1 && days.DayOfWeek == DayOfWeek.Monday
                                   select days).ElementAt(2);

                if (checkDate == mlkDay)
                    return true;
            }

            if (checkDate.Month == 2)
            {
                DateTime presDay = (from days in daysOfYear
                                    where days.Month == 2 && days.DayOfWeek == DayOfWeek.Monday
                                    select days).ElementAt(2);

                if (checkDate == presDay)
                    return true;
            }

            DateTime goodFriday = EasterSunday(checkDate.Year).AddDays(-2);
            if (checkDate == goodFriday && goodFriday != (new DateTime(checkDate.Year, 3, 31)))
                return true;


            if (checkDate.Month == 5)
            {
                DateTime memDay = (from days in daysOfYear
                                   where days.Month == 5 && days.DayOfWeek == DayOfWeek.Monday
                                   select days).Last();
                if (checkDate == memDay)
                    return true;
            }

            if (checkDate.Month == 7)
            {
                return checkDate.Date == IndependenceDayObserved(checkDate.Year).Date;
            }

            if (checkDate.Month == 9)
            {
                DateTime laborDay = (from days in daysOfYear
                                     where days.Month == 9 && days.DayOfWeek == DayOfWeek.Monday
                                     select days).First();

                if (checkDate == laborDay)
                    return true;
            }

            if (checkDate.Month == 11)
            {
                DateTime thanksgivingDay = (from days in daysOfYear
                                            where days.Month == 11 && days.DayOfWeek == DayOfWeek.Thursday
                                            select days).ElementAt(3);

                if (checkDate == thanksgivingDay)
                    return true;
            }

            if (checkDate.Month == 12)
            {
                DateTime christmasDay = ChirstmasObserved(checkDate.Year);
                if (checkDate == christmasDay)
                    return true;
            }
            return false;
        }

        private static List<DateTime> GetDateRange(DateTime StartingDate, DateTime EndingDate)
        {
            if (StartingDate > EndingDate)
                return null;

            List<DateTime> rv = new List<DateTime>();
            DateTime tmpDate = StartingDate;
            do
            {
                rv.Add(tmpDate);
                tmpDate = tmpDate.AddDays(1);
            } while (tmpDate <= EndingDate);
            return rv;
        }

        public static DateTime ChirstmasObserved(int year)
        {
            DateTime actualChristmas = new DateTime(year, 12, 25);
            switch (actualChristmas.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    return actualChristmas.AddDays(1);
                case DayOfWeek.Saturday:
                    return actualChristmas.AddDays(-1);
                default:
                    return actualChristmas;
            }
        }

        public static DateTime NewYearsObserved(int year)
        {
            DateTime actualNewYears = new DateTime(year, 1, 1);
            switch (actualNewYears.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    return actualNewYears.AddDays(1);
                case DayOfWeek.Saturday:
                    return actualNewYears.AddDays(-1);
                default:
                    return actualNewYears;
            }
        }

        public static DateTime IndependenceDayObserved(int year)
        {
            DateTime actualFourth = new DateTime(year, 7, 4);

            switch (actualFourth.DayOfWeek)
            {
                case DayOfWeek.Saturday:
                    return actualFourth.AddDays(-1);
                case DayOfWeek.Sunday:
                    return actualFourth.AddDays(1);
                default:
                    return actualFourth;
            }
        }

        public static DateTime EasterSunday(int year)
        {
            int day = 0; int month = 0;
            int g = year % 19;
            int c = year / 100;
            int h = (c - (int)(c / 4) - (int)((8 * c + 13) / 25) + 19 * g + 15) % 30;
            int i = h - (int)(h / 28) * (1 - (int)(h / 28) * (int)(29 / (h + 1)) * (int)((21 - g) / 11));
            day = i - ((year + (int)(year / 4) + i + 2 - c + (int)(c / 4)) % 7) + 28;
            month = 3;
            if (day > 31)
            {
                month++;
                day -= 31;
            }
            return new DateTime(year, month, day);
        }

        /// <summary>
        /// Returns a date X business days before the inputDate
        /// </summary>
        public static DateTime DaysPrior(DateTime inputDate, int days)
        {
            DateTime outputDate = inputDate;
            for (int i = 0; i < days; i++)
                outputDate = PreviousNYSETradingDay(outputDate);
            return outputDate;
        }
        /// <summary>
        /// Returns a date X business days after the inputDate
        /// </summary>
        public static DateTime DaysAfter(DateTime inputDate, int days)
        {
            DateTime outputDate = inputDate;
            for (int i = 0; i < days; i++)
                outputDate = NextNYSETradingDay(outputDate);
            return outputDate;
        }
    }
}
