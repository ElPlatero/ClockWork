using System;
using Clockwork.Lib.Models;

namespace Clockwork.Lib.Calculators
{
    public interface IEffectiveWorkingTimeCalculator
    {
        CalculationResultCollection Calculate(ClockWorkUnitCollection units);
        CalculationResult GetBalance(ClockWorkUnitCollection units, DateTime snapshot);
    }
}