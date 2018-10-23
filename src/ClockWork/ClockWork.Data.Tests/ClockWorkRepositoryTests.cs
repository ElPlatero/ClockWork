using System;
using System.IO;
using System.Linq;
using Clockwork.Lib.Models;
using Microsoft.Extensions.Options;
using Xunit;

namespace ClockWork.Data.Tests
{
    public class ClockWorkRepositoryTests : IDisposable
    {
        private readonly string _databaseName;
        private readonly IOptions<LiteDbOptions> _options;

        public ClockWorkRepositoryTests()
        {
            _databaseName = Guid.NewGuid().ToString("N") + ".db";
            _options = new OptionsWrapper<LiteDbOptions>(new LiteDbOptions { DatabaseFile = _databaseName });
        }

        public void Dispose()
        {
            File.Delete(_databaseName);
        }


        [Fact]
        public void WorkerTest()
        {
            var repository = new LiteClockWorkRepository(_options);
            var newWorker = new ClockWorker("Mustermann", "Max", new DateTime(1970, 5, 10));
            repository.Save(newWorker);

            var worker = repository.LoadWorker(newWorker.Id);

            newWorker = new ClockWorker("Mustermann", "Moritz", new DateTime(1970, 5, 10)) { Id = newWorker.Id };
            repository.Save(newWorker);
            var otherWorker = repository.LoadWorker(newWorker.Id);

            Assert.Equal(worker.Id, otherWorker.Id);
            Assert.NotEqual(worker.GivenName, otherWorker.GivenName);

            var allWorkers = repository.LoadWorkers();
            Assert.Single(allWorkers);
            Assert.Equal(otherWorker, allWorkers.First());
        }

        [Fact]
        public void CalendarTest()
        {
            var repository = new LiteClockWorkRepository(_options);
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
                new ClockWorkUnit(new DateTime(2018, 10, 19, 7, 35, 0), new DateTime(2018, 10, 19, 17, 11, 0))
            );

            repository.Save(calendar);
            repository.Save(calendar);
            var savedCalendar = repository.LoadCalendar(worker.Id, new DateTime(2018, 10, 8), new DateTime(2018, 10, 12));
        }

    }
}
