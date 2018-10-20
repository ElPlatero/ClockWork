using Clockwork.Lib.Models;

namespace Clockwork.Lib.Calculators
{
    public interface IEffectiveWorkingTimeCalculator
    {
        CalculationResult Calculate(ClockWorkUnitCollection units);
    }
}