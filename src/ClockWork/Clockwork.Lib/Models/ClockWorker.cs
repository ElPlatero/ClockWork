using System;
using System.Text;

namespace Clockwork.Lib.Models
{
    public class ClockWorker
    {
        public string FamilyName { get; set; }
        public string GivenName { get; set; }
        public DateTime DateOfBirth { get; set; }
    }

    public class ClockWorkUnit
    {
        private readonly IClockWorkCalculator _calculator;

        public ClockWorkUnit(IClockWorkCalculator calculator)
        {
            _calculator = calculator;
        }

        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public CalculationResult CalculateWork()
        {
            return _calculator.Calculate(this);
        }
    }

    public interface IClockWorkCalculator
    {
        CalculationResult Calculate(ClockWorkUnit unit);
    }

    public class ExactWorkCalculator : IClockWorkCalculator
    {
        public CalculationResult Calculate(ClockWorkUnit unit)
        {
            return new CalculationResult(unit.End - unit.Start, unit.End - unit.Start);
        }
    }

    public class StsWorkCalculator : IClockWorkCalculator
    {
        public CalculationResult Calculate(ClockWorkUnit unit)
        {
            var start = RoundDate(unit.Start, Math.Ceiling);
            var end = RoundDate(unit.End, Math.Floor);


            return new CalculationResult(unit.End - unit.Start, end > start ? end - start : TimeSpan.Zero);
        }

        private static DateTime RoundDate(DateTime date, Func<double, double> approximate)
        {
            var minutes = approximate(date.TimeOfDay.TotalMinutes / 10.0) * 10;
            return date.Date.AddMinutes(minutes);
        }
    }

    public class CalculationResult
    {
        public CalculationResult(TimeSpan exactDuration, TimeSpan calculatedDuration)
        {
            ExactDuration = exactDuration;
            CalculatedDuration = calculatedDuration;
        }
        public TimeSpan ExactDuration { get;  }
        public TimeSpan CalculatedDuration { get; }
    }
}
