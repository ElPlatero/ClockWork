using System;
using System.Linq;

namespace Clockwork.Lib.Calculators
{
    public class CalculationGrouping
    {
        public CalculationGrouping(CalculationResultCollection result)
        {
            Day = result;
        }

        public CalculationResultCollection Day { get; }
        public CalculationResultCollection Week => GroupCollection(Day, GetWeek);
        public CalculationResultCollection Month => GroupCollection(Day, GetMonth);
        public CalculationResultCollection Year => GroupCollection(Day, GetYear);

        private static CalculationResultCollection GroupCollection(CalculationResultCollection source, Func<DateTime, DateTime> getGrouping)
        {
            var result = new CalculationResultCollection();
            result.AddRange(source
                .GroupBy(p => getGrouping(p.Date))
                .Select(p => new CalculationResult(
                    p.Key,
                    TimeSpan.FromSeconds(p.Sum(q => q.WorkingHours.TotalSeconds)),
                    TimeSpan.FromSeconds(p.Sum(q => q.ExactWorkedHours.TotalSeconds)),
                    TimeSpan.FromSeconds(p.Sum(q => q.CalculatedWorkedHours.TotalSeconds))
                )));
            return result;
        }

        private static DateTime GetWeek(DateTime date)
        {
            while (date.DayOfWeek != DayOfWeek.Monday) date = date.AddDays(-1);
            return date;
        }

        private static DateTime GetMonth(DateTime date) => new DateTime(date.Year, date.Month, 1);
        private static DateTime GetYear(DateTime date) => new DateTime(date.Year, 1, 1);
    }
}