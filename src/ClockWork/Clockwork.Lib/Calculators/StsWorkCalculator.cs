using System;
using System.Linq;
using Clockwork.Lib.Models;

namespace Clockwork.Lib.Calculators
{
    public class StsWorkCalculator : IEffectiveWorkingTimeCalculator
    {
        private CalculationResult Calculate(ClockWorkUnit unit)
        {
            var start = RoundDate(unit.Start, Math.Ceiling);
            var end = RoundDate(unit.End, Math.Floor);

            var pause = unit.Pause;
            var calculatedDuration = end - start;
            if (calculatedDuration <= TimeSpan.Zero) pause = TimeSpan.Zero;
            if (calculatedDuration > TimeSpan.FromHours(6) && pause < TimeSpan.FromMinutes(30)) pause = TimeSpan.FromMinutes(30);
            if (calculatedDuration > TimeSpan.FromHours(9) && pause < TimeSpan.FromMinutes(45)) pause = TimeSpan.FromMinutes(45);

            calculatedDuration -= pause;

            if (calculatedDuration > TimeSpan.FromHours(10)) calculatedDuration = TimeSpan.FromHours(10);
            else if(calculatedDuration < TimeSpan.Zero) calculatedDuration = TimeSpan.Zero;

            return new CalculationResult(unit.End - unit.Start - pause, calculatedDuration);
        }

        private static DateTime RoundDate(DateTime date, Func<double, double> approximate)
        {
            var minutes = approximate(date.TimeOfDay.TotalMinutes / 10.0) * 10;
            return date.Date.AddMinutes(minutes);
        }

        public CalculationResult Calculate(ClockWorkUnitCollection units)
        {
            return units.Aggregate(new CalculationResult(TimeSpan.Zero, TimeSpan.Zero), (sum, unit) =>
            {
                var result = Calculate(unit);
                return new CalculationResult(sum.ExactDuration + result.ExactDuration, sum.CalculatedDuration + result.CalculatedDuration);
            });
        }
    }
}