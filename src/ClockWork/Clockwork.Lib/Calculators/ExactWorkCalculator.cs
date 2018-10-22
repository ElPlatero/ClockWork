using System;
using System.Linq;
using Clockwork.Lib.Models;

namespace Clockwork.Lib.Calculators
{
    public class ExactWorkCalculator : IEffectiveWorkingTimeCalculator
    {
        private static readonly TimeSpan _workingDay = TimeSpan.FromHours(8);

        private CalculationResult Calculate(ClockWorkUnit unit)
        {
            return new CalculationResult(unit.Start.Date, _workingDay, unit.End - unit.Start, unit.End - unit.Start);
        }

        public CalculationResultCollection Calculate(ClockWorkUnitCollection units)
        {
            return new CalculationResultCollection(units.Select(p => Calculate(p)));
        }
    }
}