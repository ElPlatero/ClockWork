using System;
using System.Linq;
using Clockwork.Lib.Models;

namespace Clockwork.Lib.Calculators
{
    public class ExactWorkCalculator : IEffectiveWorkingTimeCalculator
    {
        private static readonly TimeSpan WorkingDay = TimeSpan.FromHours(8);

        private static CalculationResult Calculate(ClockWorkUnit unit)
        {
            return new CalculationResult(unit.Start.Date, WorkingDay, unit.End - unit.Start, unit.End - unit.Start);
        }

        public CalculationResultCollection Calculate(ClockWorkUnitCollection units)
        {
            return new CalculationResultCollection(units.Select(Calculate));
        }
    }
}