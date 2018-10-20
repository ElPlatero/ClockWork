using System;
using Clockwork.Lib.Calculators;
using Clockwork.Lib.Models;
using Xunit;

namespace ClockWork.Lib.Tests
{
    public class StsWorkCalculatorTests
    {
        [Fact]
        public void Test()
        {
            var unit = new ClockWorkUnit(DateTime.Today.AddHours(8), DateTime.Today.AddHours(17)).PauseFor(30);
            var worker = new ClockWorker("Mustermann", "Max", DateTime.Today.AddYears(-21));
            var calendar = new ClockWorkUnitCollection(worker, unit);

            IEffectiveWorkingTimeCalculator calculator = new StsWorkCalculator();
            var calculationResult = calculator.Calculate(calendar);

            Assert.Equal(TimeSpan.FromHours(8.5), calculationResult.CalculatedDuration);
            Assert.Equal(TimeSpan.FromHours(8.5), calculationResult.ExactDuration);

            calendar.Add(new ClockWorkUnit(new DateTime(2018, 10, 1, 7, 58, 10), new DateTime(2018, 10, 1, 16, 56, 20)));
            calculationResult = calculator.Calculate(calendar);
            Assert.Equal(TimeSpan.FromMinutes(1010), calculationResult.CalculatedDuration);
            Assert.NotEqual(calculationResult.CalculatedDuration, calculationResult.ExactDuration);

        }
    }
}
