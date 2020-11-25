using System;
using System.Collections.Generic;
using System.Globalization;

namespace DateRange
{
    class Program
    {
        static List<DateTime[]> result = new List<DateTime[]>();
        static int intervalsCount = 0;
        static void Main(string[] args)
        {
            string type;
            string[] dates;

            if (args.Length == 3)
            {
                type = args[0];
                dates = new string[2];
                dates[0] = args[1];
                dates[1] = args[2];
            }
            else
            {
                Console.Write("type: ");
                type = Console.ReadLine();

                Console.Write("startDate endDate: ");
                dates = Console.ReadLine().Split(" ");
            }

            if (dates.Length == 1)
            {
                throw new Exception("endDate is missing");
            }

            CultureInfo culture = new CultureInfo("ru-RU");
            DateTime startDate;
            DateTime endDate;

            if (!DateTime.TryParse(dates[0], culture, DateTimeStyles.None, out startDate))
            {
                throw new Exception("startDate is invalid");
            }
            if (!DateTime.TryParse(dates[1], culture, DateTimeStyles.None, out endDate))
            {
                throw new Exception("endDate is invalid");
            }

            if (startDate > endDate)
            {
                throw new Exception("startDate > endDate");
            }

            switch (type)
            {
                case "WEEK":
                    IntervalByWeeks(startDate, endDate);
                    break;
                case "MONTH":
                    IntervalByMonths(startDate, endDate);
                    break;
                case "QUARTER":
                    IntervalByQuarters(startDate, endDate);
                    break;
                case "YEAR":
                    IntervalByYears(startDate, endDate);
                    break;
                case "REVIEW":
                    IntervalByReviews(startDate, endDate);
                    break;
                default:
                    Console.WriteLine("unknown type");
                    break;
            }

            Console.WriteLine(intervalsCount);
            result.ForEach(Print);

            void Print(DateTime[] interval)
            {
                Console.WriteLine($"{interval[0].ToString("yyyy-MM-dd")} {interval[1].ToString("yyyy-MM-dd")}");
            }
        }

        static void IntervalByReviews(DateTime startDate, DateTime endDate)
        {
            for (DateTime i = startDate; i <= endDate;)
            {
                DateTime[] interval = new DateTime[2];
                interval[0] = i;

                if (i.Month >= 4 && i.Month <= 9)
                {
                    i = new DateTime(i.Year, 9, DateTime.DaysInMonth(i.Year, 9));
                }
                else
                {
                    i = new DateTime(i.Year + 1, 3, DateTime.DaysInMonth(i.Year + 1, 3));
                }

                interval[1] = i;
                if (i > endDate)
                {
                    interval[1] = endDate;
                }

                result.Add(interval);
                i = i.AddDays(1);
                intervalsCount++;
            }
        }

        static void IntervalByQuarters(DateTime startDate, DateTime endDate)
        {
            for (DateTime i = startDate; i <= endDate;)
            {
                DateTime[] interval = new DateTime[2];
                int monthsToAdd = 3 - (i.Month % 3);
                if (monthsToAdd == 3)
                {
                    monthsToAdd = 0;
                }
                interval[0] = i;
                i = i.AddMonths(monthsToAdd);
                i = new DateTime(i.Year, i.Month, DateTime.DaysInMonth(i.Year, i.Month));
                interval[1] = i;
                i = i.AddDays(1);

                result.Add(interval);
                intervalsCount++;
            }
        }

        static void IntervalByYears(DateTime startDate, DateTime endDate)
        {
            for (DateTime i = startDate; i <= endDate;)
            {
                DateTime[] interval = new DateTime[2];

                interval[0] = i;
                i = new DateTime(i.Year, 12, 31);
                if (i > endDate)
                {
                    interval[1] = endDate;
                }
                else
                {
                    interval[1] = i;
                }

                i = i.AddDays(1);

                result.Add(interval);
                intervalsCount++;
            }
        }

        static void IntervalByWeeks(DateTime startDate, DateTime endDate)
        {
            for (DateTime i = startDate; i <= endDate;)
            {
                DateTime[] interval = new DateTime[2];

                int weekDay = (int) i.DayOfWeek;
                if (weekDay == 0)
                {
                    weekDay = 7;
                }
                int daysToAdd = 7 - weekDay;

                interval[0] = i;
                interval[1] = i.AddDays(daysToAdd);

                if (interval[1] >= endDate)
                {
                    interval[1] = endDate;
                }

                result.Add(interval);

                i = interval[1].AddDays(1);
                intervalsCount++;
            }
        }

        static void IntervalByMonths(DateTime startDate, DateTime endDate)
        {
            for (DateTime i = startDate; i <= endDate; i = i.AddMonths(1))
            {   
                DateTime[] interval = new DateTime[2];
                if (i == startDate) 
                {
                    interval[0] = startDate;
                }
                else 
                {
                    interval[0] = new DateTime(i.Year, i.Month, 1);
                }

                if (i.Month == endDate.Month)
                {
                    interval[1] = endDate;
                }
                else
                {
                    interval[1] = new DateTime(i.Year, i.Month, DateTime.DaysInMonth(i.Year, i.Month));
                }

                result.Add(interval);
                intervalsCount++;
            }
        }
    }
}
