using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DLeh.Util
{
    public static class DateUtil
    {
        public static DateTime GetBeginOfOctoberFiscalYear(DateTime currentDate)
        {
            var thisYearFiscalStart = new DateTime(currentDate.Year, 10, 1);

            DateTime fiscalYearStartToUse;
            if (currentDate.Date < thisYearFiscalStart)
                fiscalYearStartToUse = thisYearFiscalStart.AddYears(-1);
            else
                fiscalYearStartToUse = thisYearFiscalStart;

            return MoveBackToNearestBusinessday(fiscalYearStartToUse.AddDays(-1)).AddDays(1);
        }

        public static DateTime MoveBackToNearestBusinessday(DateTime dateToMove)
        {
            while (BusinessDayChecker.IsNYSEHolidayOrClosed(dateToMove))
                dateToMove = dateToMove.AddDays(-1);

            return dateToMove;
        }

        public static DateTime LastBusinessDayOfMonth(int year, int month)
        {
            if (year < 0)
                throw new ArgumentOutOfRangeException();

            if (month < 1 || month > 12)
                throw new ArgumentOutOfRangeException();

            return MoveBackToNearestBusinessday(new DateTime(year, month, 1).AddMonths(1).AddDays(-1));
        }

        public static DateTime BeginFirstQuarterOf(int year)
        {
            return EndFourthQuarterOf(year - 1).AddDays(1);
        }

        public static DateTime EndFirstQuarterOf(int year)
        {
            if (year < DateTime.MinValue.Year || year > DateTime.MaxValue.Year)
                throw new ArgumentOutOfRangeException(String.Format("Year cannot be below {0} or over {1}", DateTime.MinValue.Year, DateTime.MaxValue.Year));

            return new DateTime(year, 3, 31);
        }

        public static DateTime BeginSecondQuarterOf(int year)
        {
            return EndFirstQuarterOf(year).AddDays(1);
        }

        public static DateTime EndSecondQuarterOf(int year)
        {
            if (year < DateTime.MinValue.Year || year > DateTime.MaxValue.Year)
                throw new ArgumentOutOfRangeException(String.Format("Year cannot be below {0} or over {1}", DateTime.MinValue.Year, DateTime.MaxValue.Year));

            return new DateTime(year, 6, 30);
        }

        public static DateTime BeginThirdQuarterOf(int year)
        {
            return EndSecondQuarterOf(year).AddDays(1);
        }

        public static DateTime EndThirdQuarterOf(int year)
        {
            if (year < DateTime.MinValue.Year || year > DateTime.MaxValue.Year)
                throw new ArgumentOutOfRangeException(String.Format("Year cannot be below {0} or over {1}", DateTime.MinValue.Year, DateTime.MaxValue.Year));

            return new DateTime(year, 9, 30);
        }

        public static DateTime BeginFourthQuarterOf(int year)
        {
            return EndThirdQuarterOf(year).AddDays(1);
        }

        public static DateTime EndFourthQuarterOf(int year)
        {
            if (year < DateTime.MinValue.Year || year > DateTime.MaxValue.Year)
                throw new ArgumentOutOfRangeException(String.Format("Year cannot be below {0} or over {1}", DateTime.MinValue.Year, DateTime.MaxValue.Year));

            return new DateTime(year, 12, 31);
        }

        public static int GetQuarter(DateTime date)
        {
            var currentDate = date.Date;
            int currYear = currentDate.Year;

            var lastDayQ1 = MoveBackToNearestBusinessday(EndFirstQuarterOf(currYear));
            if (currentDate <= lastDayQ1)
                return 1;

            var lastDayQ2 = MoveBackToNearestBusinessday(EndSecondQuarterOf(currYear));
            if (currentDate <= lastDayQ2)
                return 2;

            var lastDayQ3 = MoveBackToNearestBusinessday(EndThirdQuarterOf(currYear));
            if (currentDate <= lastDayQ3)
                return 3;

            return 4;
        }

        public static int GetPreviousQuarter(int quarter)
        {
            if (quarter == 1)
                return 4;
            return quarter - 1;
        }

        public static string GetOrdinal(int num)
        {
            if (num <= 0) return num.ToString();

            switch (num % 100)
            {
                case 11:
                case 12:
                case 13:
                    return num + "th";
            }

            switch (num % 10)
            {
                case 1:
                    return num + "st";
                case 2:
                    return num + "nd";
                case 3:
                    return num + "rd";
                default:
                    return num + "th";
            }

        }

        public static DateTime GetCurrentOrPreviousSunday(this DateTime d)
        {
            while (d.DayOfWeek != DayOfWeek.Sunday)
                d = d.AddDays(-1);
            return d;
        }

        public static int YearsSince(this DateTime date, DateTime since)
        {
            if (since > date)
                throw new ArgumentException($"{nameof(since)} cannot be greater than {nameof(date)}");

            int yearsSince = date.Year - since.Year;
            if (since > date.AddYears(-yearsSince)) yearsSince--;
            return yearsSince;
        }

        //http://stackoverflow.com/a/288542/526704
        public static DateTime FindTheNthSpecificWeekday(int year, int month, int nth, DayOfWeek dayOfWeek)
        {
            // validate month value
            if (month < 1 || month > 12)
            {
                throw new ArgumentOutOfRangeException("Invalid month value.");
            }

            // validate the nth value
            if (nth < 0 || nth > 5)
            {
                throw new ArgumentOutOfRangeException("Invalid nth value.");
            }

            // start from the first day of the month
            DateTime dt = new DateTime(year, month, 1);

            // loop until we find our first match day of the week
            while (dt.DayOfWeek != dayOfWeek)
            {
                dt = dt.AddDays(1);
            }

            if (dt.Month != month)
            {
                // we skip to the next month, we throw an exception
                throw new ArgumentOutOfRangeException(string.Format("The given month has less than {0} {1}s", nth, dayOfWeek));
            }

            // Complete the gap to the nth week
            dt = dt.AddDays((nth - 1) * 7);

            return dt;
        }
    }
}
