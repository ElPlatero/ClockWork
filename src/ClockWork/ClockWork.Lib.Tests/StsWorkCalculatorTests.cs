using System;
using System.Globalization;
using System.Linq;
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

            Assert.Equal("08.10.2018 07:36 - 08.10.2018 15:32 (Pause: 00:30)", calendar.First().ToString());

            IEffectiveWorkingTimeCalculator calculator = new StsWorkCalculator();
            var calculationResult = calculator.Calculate(calendar);

            Assert.Equal(TimeSpan.FromHours(45.25), calculationResult.CalculatedWorkedHours);
            Assert.True(calculationResult.ExactWorkedHours > calculationResult.CalculatedWorkedHours);

            _output.WriteLine($"{calculationResult.CalculatedWorkedHours:g} ({calculationResult.ExactWorkedHours:g})");

            var balance = calculator.GetBalance(calendar, new DateTime(2018, 10, 10, 0, 0, 0));
            Assert.Equal(TimeSpan.FromMinutes(985), balance.CalculatedWorkedHours);
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
                new ClockWorkUnit(new DateTime(2018, 10, 01, 07, 38, 0), new DateTime(2018, 10, 01, 16, 12, 0)),
                new ClockWorkUnit(new DateTime(2018, 10, 02, 07, 33, 0), new DateTime(2018, 10, 02, 17, 00, 0)),
                new ClockWorkUnit(new DateTime(2018, 10, 04, 07, 35, 0), new DateTime(2018, 10, 04, 17, 07, 0)),
                new ClockWorkUnit(new DateTime(2018, 10, 05, 07, 36, 0), new DateTime(2018, 10, 05, 17, 05, 0)),
                new ClockWorkUnit(new DateTime(2018, 10, 08, 06, 58, 0), new DateTime(2018, 10, 08, 15, 35, 0)),
                new ClockWorkUnit(new DateTime(2018, 10, 09, 07, 33, 0), new DateTime(2018, 10, 09, 17, 01, 0)),
                new ClockWorkUnit(new DateTime(2018, 10, 10, 07, 35, 0), new DateTime(2018, 10, 10, 21, 00, 0)),
                new ClockWorkUnit(new DateTime(2018, 10, 11, 08, 30, 0), new DateTime(2018, 10, 11, 21, 00, 0)),
                new ClockWorkUnit(new DateTime(2018, 10, 12, 07, 35, 0), new DateTime(2018, 10, 12, 16, 45, 0)),
                new ClockWorkUnit(new DateTime(2018, 10, 15, 07, 42, 0), new DateTime(2018, 10, 15, 17, 00, 0)),
                new ClockWorkUnit(new DateTime(2018, 10, 16, 07, 38, 0), new DateTime(2018, 10, 16, 15, 42, 0)),
                new ClockWorkUnit(new DateTime(2018, 10, 17, 07, 33, 0), new DateTime(2018, 10, 17, 17, 08, 0)),
                new ClockWorkUnit(new DateTime(2018, 10, 18, 07, 34, 0), new DateTime(2018, 10, 18, 17, 00, 0)),
                new ClockWorkUnit(new DateTime(2018, 10, 19, 07, 35, 0), new DateTime(2018, 10, 19, 17, 11, 0)),
                new ClockWorkUnit(new DateTime(2018, 10, 22, 07, 38, 0), new DateTime(2018, 10, 22, 16, 12, 0)),
                new ClockWorkUnit(new DateTime(2018, 10, 23, 07, 45, 0), new DateTime(2018, 10, 23, 17, 00, 0)),
                new ClockWorkUnit(new DateTime(2018, 10, 24, 00, 00, 0), new DateTime(2018, 10, 24, 00, 00, 0)),
                new ClockWorkUnit(new DateTime(2018, 10, 25, 07, 40, 0), new DateTime(2018, 10, 25, 16, 36, 0)),
                new ClockWorkUnit(new DateTime(2018, 10, 26, 07, 32, 0), new DateTime(2018, 10, 26, 17, 02, 0)),
                new ClockWorkUnit(new DateTime(2018, 10, 29, 07, 40, 0), new DateTime(2018, 10, 29, 16, 10, 0)),
                new ClockWorkUnit(new DateTime(2018, 10, 30, 07, 35, 0), new DateTime(2018, 10, 30, 17, 02, 0))
            );

            IEffectiveWorkingTimeCalculator calculator = new StsWorkCalculator();

            var calculationResult = calculator.Calculate(calendar);

            string DisplayTime(TimeSpan ts)
            {
                return $"{(ts < TimeSpan.Zero ? "-" : "+")}{(int) Math.Abs(ts.TotalHours):D2}:{Math.Abs(ts.Minutes):D2}";
            }

            void LogResult(CalculationResultCollection r, Func<CalculationResult, string> getPrefix)
            {
                r.ToList().ForEach(p => _output.WriteLine("{0}: balance {1}", getPrefix(p), DisplayTime(p.Overtime)));
            }

            _output.WriteLine($"Calculated working time for calendar from {calendar.First().Start.Date:dd.MM.yyyy} to {calendar.Last().Start.Date:dd.MM.yyyy}");
            _output.WriteLine($"    Regular working hours: {DisplayTime(calculationResult.WorkingHours)}");
            _output.WriteLine($"    Hours accounted for: {DisplayTime(calculationResult.CalculatedWorkedHours)}");
            _output.WriteLine($"    Unaccounted time: {DisplayTime(calculationResult.ExactWorkedHours - calculationResult.CalculatedWorkedHours)}");
            _output.WriteLine($"    Accounted overtime: {DisplayTime(calculationResult.Overtime)}");
            _output.WriteLine("".PadLeft(80, '='));
            _output.WriteLine(string.Empty);

            _output.WriteLine("Output by day:");
            LogResult(calculationResult, p => p.Date.ToString("dd.MM.yyyy"));
            _output.WriteLine("".PadLeft(80, '='));

            Assert.Equal(21, calculationResult.Count);
            Assert.Equal(calculationResult, calculationResult.GroupBy.Day);

            var result = calculationResult.GroupBy.Week;
            _output.WriteLine("Output by week:");
            LogResult(result, p => "KW " + (1 + (p.Date.DayOfYear - 1) / 7));
            _output.WriteLine("".PadLeft(80, '='));
            Assert.Equal(5, result.Count);

            result = calculationResult.GroupBy.Month;
            _output.WriteLine("Output by month:");
            LogResult(result, p => p.Date.ToString("MMMM", new CultureInfo("de-DE")));
            _output.WriteLine("".PadLeft(80, '='));
            Assert.Single(result);

            result = calculationResult.GroupBy.Year;
            _output.WriteLine("Output by year:");
            LogResult(result, p => p.Date.Year.ToString(CultureInfo.InvariantCulture));
            _output.WriteLine("".PadLeft(80, '='));
            Assert.Single(result);
        }
    }
}
