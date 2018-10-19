using System;
using System.Collections.Generic;
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
