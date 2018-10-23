using System;
using System.Collections.Generic;
using System.Linq;
using Clockwork.Lib.Models;
using Clockwork.Lib.Repositories;
using ClockWork.Data.Models;
using LiteDB;
using Microsoft.Extensions.Options;

namespace ClockWork.Data
{
    public class LiteClockWorkRepository : IClockWorkRepository
    {
        private readonly LiteDbOptions _options;
        private readonly List<ClockWorkUnitCollection> _loadedCollections = new List<ClockWorkUnitCollection>();
        private static ClockWorkDatabase GetDatabase(LiteDbOptions options) => new ClockWorkDatabase(options.DatabaseFile);


        public LiteClockWorkRepository(IOptions<LiteDbOptions> options)
        {
            _options = options.Value;

            var mapper = BsonMapper.Global;
            mapper.Entity<Unit>().DbRef(p => p.Worker, "workers");
        }

        public ClockWorkUnitCollection LoadCalendar(int workerId)
        {
            var loadedCalendar = _loadedCollections.FirstOrDefault(p => p.Worker.Id == workerId);
            if (loadedCalendar != null) return loadedCalendar; 

            using (var db = GetDatabase(_options))
            {
                var units = db.Units;

                var query = units
                    .Include(p => p.Worker)
                    .Find(p => p.Worker.Id == workerId).ToArray();

                if (!query.Any()) return null;
                loadedCalendar = new ClockWorkUnitCollection(query[0].Worker.ToBusinessModel(), query.Select(p => p.ToBusinessModel()).ToArray());
                _loadedCollections.Add(loadedCalendar);
                return loadedCalendar;
            }
        }

        public ClockWorker LoadWorker(int workerId)
        {
            using (var db = GetDatabase(_options))
            {
                var workers = db.Workers;

                var foundWorker = workers.FindById(workerId);
                return foundWorker?.ToBusinessModel();
            }
        }

        public ClockWorker[] LoadWorkers()
        {
            using (var db = GetDatabase(_options))
            {
                var workers = db.Workers;
                return workers.FindAll().Select(p => p.ToBusinessModel()).ToArray();
            }
        }

        public void Save(ClockWorkUnitCollection calendar)
        {
            var existingCalender = _loadedCollections.SingleOrDefault(p => p.Worker.Equals(calendar.Worker));
            if (existingCalender != null)
            {
                _loadedCollections.Remove(existingCalender);
                _loadedCollections.Add(calendar);
            }

            using (var db = GetDatabase(_options))
            {
                var workers = db.Workers;
                var units = db.Units;

                var foundWorker = workers.FindOne(p => p.Id == calendar.Worker.Id);

                if (foundWorker == null)
                {
                    Save(calendar.Worker);
                    foundWorker = Worker.FromBusinessModel(calendar.Worker);
                }

                units.EnsureIndex(p => p.Start);
                units.EnsureIndex(p => p.End);

                units.Delete(p => p.Start >= calendar.First().Start.Date && p.End < calendar.Last().End.Date.AddDays(1) && p.Worker.Id == foundWorker.Id);
                units.InsertBulk(calendar.Select(p => Unit.FromBusinessModel(p, foundWorker)));
            }
        }

        public void Save(ClockWorker worker)
        {
            using (var db = GetDatabase(_options))
            {
                var workers = db.Workers;

                if (worker.Id == 0)
                {
                    worker.Id = workers.Insert(Worker.FromBusinessModel(worker));
                }
                else
                {
                    workers.Update(Worker.FromBusinessModel(worker));
                }
            }
        }
    }
}
