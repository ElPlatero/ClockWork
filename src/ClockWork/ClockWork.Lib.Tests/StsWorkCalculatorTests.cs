using System;
using Clockwork.Lib.Calculators;
using Clockwork.Lib.Models;
using Xunit;
using Xunit.Abstractions;

namespace ClockWork.Lib.Tests
{
    public class StsWorkCalculatorTests
    {
        private readonly ITestOutputHelper _output;

        public StsWorkCalculatorTests(ITestOutputHelper output)
        {
            _output = output;
        }

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

        [Fact]
        public void ComplexText()
        {
            var calendar = new ClockWorkUnitCollection(
                new ClockWorker("Mustermann", "Max", new DateTime(1976,4,3)),
                new ClockWorkUnit(new DateTime(2018, 10, 8, 7, 36, 0), new DateTime(2018, 10, 8, 15, 32, 0)).PauseFor(30), //-00:40:00
                new ClockWorkUnit(new DateTime(2018, 10, 9, 7, 32, 0), new DateTime(2018, 10, 9, 17, 38, 0)).PauseFor(30),  //+01:05:00             
                new ClockWorkUnit(new DateTime(2018, 10, 10, 7, 35, 0), new DateTime(2018, 10, 10, 21, 30, 0)).PauseFor(30).PauseFor(15).PauseFor(15), //+02:00:00
                new ClockWorkUnit(new DateTime(2018, 10, 11, 8, 0, 0), new DateTime(2018, 10, 11, 21, 0, 0)).PauseFor(60), //+02:00:00
                new ClockWorkUnit(new DateTime(2018, 10, 12, 7, 38, 0), new DateTime(2018, 10, 12, 17, 1, 0)).PauseFor(30) //+00:50:00
            );

            IEffectiveWorkingTimeCalculator calculator = new StsWorkCalculator();
            var calculationResult = calculator.Calculate(calendar);

            Assert.Equal(TimeSpan.FromHours(45.25), calculationResult.CalculatedDuration);
            Assert.True(calculationResult.ExactDuration > calculationResult.CalculatedDuration);

            _output.WriteLine($"{calculationResult.CalculatedDuration:g} ({calculationResult.ExactDuration:g})");
        }

        [Fact]
        public void ExoticTest()
        {
            var calendar = new ClockWorkUnitCollection(
                new ClockWorker("Mustermann", "Max", new DateTime(1976, 4, 3)),
                new ClockWorkUnit(new DateTime(2018, 10, 1, 8, 2, 0), new DateTime(2018, 10, 1, 8, 5, 0)));

            IEffectiveWorkingTimeCalculator calculator = new StsWorkCalculator();
            var calculationResult = calculator.Calculate(calendar);

            Assert.Equal(TimeSpan.Zero, calculationResult.CalculatedDuration);
        }
    }
}
