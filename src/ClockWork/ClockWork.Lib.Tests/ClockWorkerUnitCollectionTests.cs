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
            var myCalendar = new ClockWorkUnitCollection(Worker, new ClockWorkUnit(DateTime.Today.AddHours(8), DateTime.Today.AddHours(17)));

            myCalendar.Add(new ClockWorkUnit(DateTime.Today.AddHours(4), DateTime.Today.AddHours(5)));
            Assert.Equal(2, myCalendar.Count);

            myCalendar.Add(new ClockWorkUnit(DateTime.Today.AddHours(7.5), DateTime.Today.AddHours(8.5)));
            Assert.Equal(2, myCalendar.Count);
            Assert.Equal(7.5, myCalendar.Last().Start.TimeOfDay.TotalHours);
            Assert.Equal(17, myCalendar.Last().End.TimeOfDay.TotalHours);

            myCalendar.Add(new ClockWorkUnit(DateTime.Today.AddHours(16.5), DateTime.Today.AddHours(17.5)));
            Assert.Equal(2, myCalendar.Count);
            Assert.Equal(7.5, myCalendar.Last().Start.TimeOfDay.TotalHours);
            Assert.Equal(17.5, myCalendar.Last().End.TimeOfDay.TotalHours);

            myCalendar.Add(new ClockWorkUnit(DateTime.Today.AddHours(7), DateTime.Today.AddHours(18)));
            Assert.Equal(2, myCalendar.Count);
            Assert.Equal(7, myCalendar.Last().Start.TimeOfDay.TotalHours);
            Assert.Equal(18, myCalendar.Last().End.TimeOfDay.TotalHours);

            myCalendar.Add(new ClockWorkUnit(DateTime.Today.AddHours(8), DateTime.Today.AddHours(17)));
            Assert.Equal(2, myCalendar.Count);
            Assert.Equal(7, myCalendar.Last().Start.TimeOfDay.TotalHours);
            Assert.Equal(18, myCalendar.Last().End.TimeOfDay.TotalHours);
        }

        [Fact]
        public void AddMultipleDaysUnitTest()
        {
            var start = new DateTime(2018,10,1);

            var myCalendar = new ClockWorkUnitCollection(Worker, new ClockWorkUnit(start.AddHours(8), start.AddHours(17)));
            myCalendar.Add(new ClockWorkUnit(start.AddHours(16), start.AddHours(100)));

            Assert.Equal(5, myCalendar.Count);
        }

        [Fact]
        public void CollectionTest()
        {
            var myCalendar = new ClockWorkUnitCollection(Worker, new ClockWorkUnit(DateTime.Today.AddHours(8), DateTime.Today.AddHours(17)));
            myCalendar.Clear();

            Assert.Empty(myCalendar);

            myCalendar.Add(new ClockWorkUnit(DateTime.Today.AddHours(8), DateTime.Today.AddHours(17)));
            Assert.Contains(myCalendar.First(), myCalendar);

            ClockWorkUnit[] myArray = new ClockWorkUnit[10];
            myCalendar.CopyTo(myArray, 1);
            Assert.True(myArray.Skip(2).All(p => p == null));
            Assert.Null(myArray.First());
            Assert.Contains(myArray[1], myCalendar);

            myCalendar.Remove(myArray[1]);
            Assert.Empty(myCalendar);
        }

        [Fact]
        public void ExceptionTest()
        {
            var myCalendar = new ClockWorkUnitCollection(Worker, new ClockWorkUnit(DateTime.Today.AddHours(8), DateTime.Today.AddHours(17)));
            Assert.Throws<ArgumentNullException>(() => myCalendar.Add(null));

        }

    }
}
