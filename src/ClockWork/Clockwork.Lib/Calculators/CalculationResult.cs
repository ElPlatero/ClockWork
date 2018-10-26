using System;

namespace Clockwork.Lib.Calculators
{
    public class CalculationResult
    {
        public CalculationResult(DateTime date, TimeSpan workingHours, TimeSpan exactDuration, TimeSpan calculatedDuration)
        {
            Date = date;
            WorkingHours = workingHours;
            ExactWorkedHours = exactDuration;
            CalculatedWorkedHours = calculatedDuration;
        }

        public DateTime Date { get; }
        public TimeSpan WorkingHours { get; }
        public TimeSpan ExactWorkedHours { get;  }
        public TimeSpan CalculatedWorkedHours { get; }

        public TimeSpan Overtime => CalculatedWorkedHours - WorkingHours;

        public override string ToString()
        {
            string DisplayTime(TimeSpan ts)
            {
                return $"{(ts < TimeSpan.Zero ? "-" : "+")}{(int) Math.Abs(ts.TotalHours):D2}:{Math.Abs(ts.Minutes):D2}";
            }



            return $"{Date:dd.MM.yyyy}, KW {1 + (Date.DayOfYear - 1) / 7}: {DisplayTime(CalculatedWorkedHours)}/{DisplayTime(WorkingHours)}";
        }
    }
}