using System;

namespace Clockwork.Lib.Calculators
{
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