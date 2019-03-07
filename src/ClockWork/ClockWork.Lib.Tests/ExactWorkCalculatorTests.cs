using System;
using System.Linq;
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
            var startDate = new DateTime(2018, 10, 26).Date;

            var unit = new ClockWorkUnit(startDate.AddHours(8), startDate.AddHours(17));
            var worker = new ClockWorker("Mustermann", "Max", startDate.AddYears(-21));

            var calendar = new ClockWorkUnitCollection(worker, unit);

            IEffectiveWorkingTimeCalculator calculator = new ExactWorkCalculator();
            var calculationResult = calculator.Calculate(calendar);

            Assert.Equal(TimeSpan.FromHours(9), calculationResult.CalculatedWorkedHours);
            Assert.Equal(TimeSpan.FromHours(9), calculationResult.ExactWorkedHours);

            var result = calculator.GetBalance(calendar, startDate.AddDays(1));
            Assert.Equal(calculationResult.CalculatedWorkedHours, result.CalculatedWorkedHours);

            Assert.Equal("26.10.2018, KW 43: +09:00/+08:00", calculationResult.Single().ToString());
        }
    }
}
