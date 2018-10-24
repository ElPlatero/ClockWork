using System;
using System.Linq;
using Clockwork.Lib.Models;
using Xunit;

namespace ClockWork.Lib.Tests
{
    public class ClockWorkerUnitCollectionTests
    {
        private static readonly ClockWorker Worker = new ClockWorker("Mustermann", "Max", DateTime.Today.AddYears(-21));

        [Fact]
        public void CreateTest()
        {
            var myCalendar = new ClockWorkUnitCollection(Worker);
            Assert.Equal(Worker, myCalendar.Worker);
            Assert.Empty(myCalendar);
            Assert.False(myCalendar.IsReadOnly);
        }

        [Fact]
        public void CreateStandardCalendarTest()
        {
            var myCalendar = new ClockWorkUnitCollection(
                Worker,
                new ClockWorkUnit(DateTime.Today.AddDays(-3).AddHours(8), DateTime.Today.AddDays(-3).AddHours(17)),
                new ClockWorkUnit(DateTime.Today.AddDays(-2).AddHours(8), DateTime.Today.AddDays(-2).AddHours(17)),
                new ClockWorkUnit(DateTime.Today.AddDays(-1).AddHours(8), DateTime.Today.AddDays(-1).AddHours(17)),
                new ClockWorkUnit(DateTime.Today.AddHours(8), DateTime.Today.AddHours(17))
            );

            Assert.Equal(4, myCalendar.Count);
        }

        [Fact]
        public void AddOverlapTest()
        {
            var calendar = new ClockWorkUnitCollection(Worker, new ClockWorkUnit(DateTime.Today.AddHours(8), DateTime.Today.AddHours(17)))
            {
                new ClockWorkUnit(DateTime.Today.AddHours(4), DateTime.Today.AddHours(5))
            };

            Assert.Equal(2, calendar.Count);

            calendar.Add(new ClockWorkUnit(DateTime.Today.AddHours(7.5), DateTime.Today.AddHours(8.5)));
            Assert.Equal(2, calendar.Count);
            Assert.Equal(7.5, calendar.Last().Start.TimeOfDay.TotalHours);
            Assert.Equal(17, calendar.Last().End.TimeOfDay.TotalHours);

            calendar.Add(new ClockWorkUnit(DateTime.Today.AddHours(16.5), DateTime.Today.AddHours(17.5)));
            Assert.Equal(2, calendar.Count);
            Assert.Equal(7.5, calendar.Last().Start.TimeOfDay.TotalHours);
            Assert.Equal(17.5, calendar.Last().End.TimeOfDay.TotalHours);

            calendar.Add(new ClockWorkUnit(DateTime.Today.AddHours(7), DateTime.Today.AddHours(18)));
            Assert.Equal(2, calendar.Count);
            Assert.Equal(7, calendar.Last().Start.TimeOfDay.TotalHours);
            Assert.Equal(18, calendar.Last().End.TimeOfDay.TotalHours);

            calendar.Add(new ClockWorkUnit(DateTime.Today.AddHours(8), DateTime.Today.AddHours(17)));
            Assert.Equal(2, calendar.Count);
            Assert.Equal(7, calendar.Last().Start.TimeOfDay.TotalHours);
            Assert.Equal(18, calendar.Last().End.TimeOfDay.TotalHours);
        }

        [Fact]
        public void AddMultipleDaysUnitTest()
        {
            var start = new DateTime(2018,10,1);

            var calendar = new ClockWorkUnitCollection(Worker, new ClockWorkUnit(start.AddHours(8), start.AddHours(17)))
            {
                new ClockWorkUnit(start.AddHours(16), start.AddHours(100))
            };

            Assert.Equal(5, calendar.Count);
        }

        [Fact]
        public void CollectionTest()
        {
            var calendar = new ClockWorkUnitCollection(Worker, new ClockWorkUnit(DateTime.Today.AddHours(8), DateTime.Today.AddHours(17)));
            calendar.Clear();

            Assert.Empty(calendar);

            calendar.Add(new ClockWorkUnit(DateTime.Today.AddHours(8), DateTime.Today.AddHours(17)));
            Assert.Contains(calendar.First(), calendar);

            var unitList = new ClockWorkUnit[10];
            calendar.CopyTo(unitList, 1);
            Assert.True(unitList.Skip(2).All(p => p == null));
            Assert.Null(unitList.First());
            Assert.Contains(unitList[1], calendar);

            calendar.Remove(unitList[1]);
            Assert.Empty(calendar);
        }

        [Fact]
        public void CreateEmptyCollectionTest()
        {
            var calendar = new ClockWorkUnitCollection(Worker);
            Assert.Empty(calendar);
            calendar = new ClockWorkUnitCollection(Worker, null);
            Assert.Empty(calendar);
            calendar.Add(new ClockWorkUnit(DateTime.Today, DateTime.Now));
            Assert.NotEmpty(calendar);
        }

        [Fact]
        public void ExceptionTest()
        {
            var calendar = new ClockWorkUnitCollection(Worker, new ClockWorkUnit(DateTime.Today.AddHours(8), DateTime.Today.AddHours(17)));
            Assert.Throws<ArgumentNullException>(() => calendar.Add(null));
        }

    }
}
