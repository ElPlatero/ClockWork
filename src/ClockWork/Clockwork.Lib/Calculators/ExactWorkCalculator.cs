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

        public CalculationResult GetBalance(ClockWorkUnitCollection units, DateTime snapshot)
        {
            var result = new CalculationResultCollection(new ClockWorkUnitCollection(units.Worker, units.Where(p => p.End <= snapshot).ToArray()).Select(Calculate));
            return new CalculationResult(units.First().Start.Date, result.WorkingHours, result.ExactWorkedHours, result.CalculatedWorkedHours);
        }
    }
}