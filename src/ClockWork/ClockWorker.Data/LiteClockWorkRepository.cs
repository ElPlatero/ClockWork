using System;
using System.Linq;
using Clockwork.Lib.Models;
using Clockwork.Lib.Repositories;
using LiteDB;
using Microsoft.Extensions.Options;

namespace ClockWork.Data
{
    public class LiteClockWorkRepository : IClockWorkRepository
    {
        private readonly LiteDbOptions _options;

        public LiteClockWorkRepository(IOptions<LiteDbOptions> options)
        {
            _options = options.Value;
        }

        public ClockWorkUnitCollection LoadCalendar(ClockWorker worker, DateTime firstDay, DateTime lastDay)
        {
            using (var db = new LiteDatabase(_options.DatabaseFile))
            {
                var foundWorker = LoadWorker(worker.FamilyName, worker.GivenName, worker.DateOfBirth);

                if (foundWorker == null) return null;

                var units = db.GetCollection<Unit>();
                units.EnsureIndex(p => p.WorkerHashcode);
                units.EnsureIndex(p => p.Start);
                units.EnsureIndex(p => p.End);

                return new ClockWorkUnitCollection(
                    foundWorker,
                    units
                        .Find(p => p.WorkerHashcode == foundWorker.GetHashCode() && p.Start >= firstDay && p.End <= lastDay)
                        .Select(p => p.ToBusinessModel()).ToArray());
            }
        }

        public ClockWorker LoadWorker(string familyName, string givenName, DateTime dateOfBirth)
        {
            using (var db = new LiteDatabase(_options.DatabaseFile))
            {
                var workers = db.GetCollection<ClockWorker>();

                workers.EnsureIndex(p => p.FamilyName);
                workers.EnsureIndex(p => p.GivenName);
                workers.EnsureIndex(p => p.DateOfBirth);

                var foundWorker = workers.FindOne(p => p.FamilyName == familyName && p.GivenName == givenName && p.DateOfBirth == dateOfBirth);

                return foundWorker;
            }
        }

        public void Save(ClockWorkUnitCollection calendar)
        {
            using (var db = new LiteDatabase(_options.DatabaseFile))
            {
                var foundWorker = LoadWorker(calendar.Worker.FamilyName, calendar.Worker.GivenName, calendar.Worker.DateOfBirth);
                if (foundWorker == null)
                {
                    db.GetCollection<ClockWorker>().Insert(calendar.Worker);
                }

                db.GetCollection<Unit>().Delete(p => p.Start >= calendar.First().Start && p.End <= calendar.Last().End && p.WorkerHashcode == foundWorker.GetHashCode());
                db.GetCollection<Unit>().InsertBulk(calendar.Select(p => Unit.FromBusinessModel(foundWorker, p)));
            }
        }

        public void Save(ClockWorker worker)
        {
            using (var db = new LiteDatabase(_options.DatabaseFile))
            {
                db.GetCollection<ClockWorker>().Insert(worker);
            }
        }
    }
}
