using System;
using System.Linq;
using Clockwork.Lib.Models;

namespace Clockwork.Lib.Calculators
{
    public class ExactWorkCalculator : IEffectiveWorkingTimeCalculator
    {
        private CalculationResult Calculate(ClockWorkUnit unit)
        {
            return new CalculationResult(unit.End - unit.Start, unit.End - unit.Start);
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