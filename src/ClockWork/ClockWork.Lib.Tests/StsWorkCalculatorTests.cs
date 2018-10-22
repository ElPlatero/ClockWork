using System;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
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

            Assert.Equal(TimeSpan.FromHours(8.5), calculationResult.CalculatedWorkedHours);
            Assert.Equal(TimeSpan.FromHours(8.5), calculationResult.ExactWorkedHours);

            calendar.Add(new ClockWorkUnit(new DateTime(2018, 10, 1, 7, 58, 10), new DateTime(2018, 10, 1, 16, 56, 20)));
            calculationResult = calculator.Calculate(calendar);
            Assert.Equal(TimeSpan.FromMinutes(1010), calculationResult.CalculatedWorkedHours);
            Assert.NotEqual(calculationResult.CalculatedWorkedHours, calculationResult.ExactWorkedHours);
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

            Assert.Equal(TimeSpan.FromHours(45.25), calculationResult.CalculatedWorkedHours);
            Assert.True(calculationResult.ExactWorkedHours > calculationResult.CalculatedWorkedHours);

            _output.WriteLine($"{calculationResult.CalculatedWorkedHours:g} ({calculationResult.ExactWorkedHours:g})");
        }

        [Fact]
        public void ExoticTest()
        {
            var calendar = new ClockWorkUnitCollection(
                new ClockWorker("Mustermann", "Max", new DateTime(1976, 4, 3)),
                new ClockWorkUnit(new DateTime(2018, 10, 1, 8, 2, 0), new DateTime(2018, 10, 1, 8, 5, 0)));

            IEffectiveWorkingTimeCalculator calculator = new StsWorkCalculator();
            var calculationResult = calculator.Calculate(calendar);

            Assert.Equal(TimeSpan.Zero, calculationResult.CalculatedWorkedHours);
        }

        [Fact]
        public void RealDataTest()
        {
            var worker = new ClockWorker("Mustermann", "Max", new DateTime(1970, 5, 10));
            var calendar = new ClockWorkUnitCollection(
                worker,
                new ClockWorkUnit(new DateTime(2018, 10, 1, 7, 38, 0), new DateTime(2018, 10, 1, 16, 12, 0)),
                new ClockWorkUnit(new DateTime(2018, 10, 2, 7, 33, 0), new DateTime(2018, 10, 2, 17, 00, 0)),
                new ClockWorkUnit(new DateTime(2018, 10, 4, 7, 35, 0), new DateTime(2018, 10, 4, 17, 7, 0)),
                new ClockWorkUnit(new DateTime(2018, 10, 5, 7, 36, 0), new DateTime(2018, 10, 5, 17, 5, 0)),
                new ClockWorkUnit(new DateTime(2018, 10, 8, 6, 58, 0), new DateTime(2018, 10, 8, 15, 35, 0)),
                new ClockWorkUnit(new DateTime(2018, 10, 9, 7, 33, 0), new DateTime(2018, 10, 9, 17, 1, 0)),
                new ClockWorkUnit(new DateTime(2018, 10, 10, 7, 35, 0), new DateTime(2018, 10, 10, 21, 0, 0)),
                new ClockWorkUnit(new DateTime(2018, 10, 11, 8, 30, 0), new DateTime(2018, 10, 11, 21, 0, 0)),
                new ClockWorkUnit(new DateTime(2018, 10, 12, 7, 35, 0), new DateTime(2018, 10, 12, 16, 45, 0)),
                new ClockWorkUnit(new DateTime(2018, 10, 15, 7, 42, 0), new DateTime(2018, 10, 15, 17, 0, 0)),
                new ClockWorkUnit(new DateTime(2018, 10, 16, 7, 38, 0), new DateTime(2018, 10, 16, 15, 42, 0)),
                new ClockWorkUnit(new DateTime(2018, 10, 17, 7, 33, 0), new DateTime(2018, 10, 17, 17, 8, 0)),
                new ClockWorkUnit(new DateTime(2018, 10, 18, 7, 34, 0), new DateTime(2018, 10, 18, 17, 0, 0)),
                new ClockWorkUnit(new DateTime(2018, 10, 19, 7, 35, 0), new DateTime(2018, 10, 19, 17, 11, 0)),
                new ClockWorkUnit(new DateTime(2018, 10, 24), new DateTime(2018, 10, 24))
            );

            IEffectiveWorkingTimeCalculator calculator = new StsWorkCalculator();

            calendar.ToList().ForEach(p => _output.WriteLine(p.ToString()));

            var result = calculator.Calculate(calendar);//.GroupByWeek();

            string DisplayTime(TimeSpan ts)
            {
                return $"{(int) ts.TotalHours:D2}:{ts.Minutes:D2}";
            }

            _output.WriteLine($"Calculated working time for calendar from {calendar.First().Start.Date:dd.MM.yyyy} to {calendar.Last().Start.Date:dd.MM.yyyy}");
            _output.WriteLine($"    Regular working hours: {DisplayTime(result.WorkingHours)}");
            _output.WriteLine($"    Hours accounted for: {DisplayTime(result.CalculatedWorkedHours)}");
            _output.WriteLine($"    Unaccounted time: {DisplayTime(result.ExactWorkedHours - result.CalculatedWorkedHours)}");
            _output.WriteLine($"    Accounted overtime: {DisplayTime(result.Overtime)}");

            Assert.Equal(15, result.Count);

            result = result.GroupByWeek();
            Assert.Equal(4, result.Count);

            result.ToList().ForEach(p => _output.WriteLine("{0} (Overtime: {1})", p, DisplayTime(p.Overtime)));
        }
    }
}
