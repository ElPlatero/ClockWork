using System;
using Clockwork.Lib.Calculators;
using Clockwork.Lib.Models;
using Xunit;

namespace ClockWork.Lib.Tests
{
    public class ExactWorkCalculatorTests
    {
        [Fact]
        public void Test()
        {
            var unit = new ClockWorkUnit(DateTime.Today.AddHours(8), DateTime.Today.AddHours(17));
            var worker = new ClockWorker("Mustermann", "Max", DateTime.Today.AddYears(-21));

            var calendar = new ClockWorkUnitCollection(worker, unit);

            IEffectiveWorkingTimeCalculator calculator = new ExactWorkCalculator();
            var calculationResult = calculator.Calculate(calendar);

            Assert.Equal(TimeSpan.FromHours(9), calculationResult.CalculatedWorkedHours);
            Assert.Equal(TimeSpan.FromHours(9), calculationResult.ExactWorkedHours);


        }
    }
}
