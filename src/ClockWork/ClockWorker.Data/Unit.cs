using System;
using Clockwork.Lib.Models;

namespace ClockWork.Data
{
    class Unit : ClockWorkUnit
    {
        public Unit(DateTime start, DateTime end) : base(start, end) { }
        public int WorkerHashcode { get; set; }

        public ClockWorkUnit ToBusinessModel() => new ClockWorkUnit(Start, End).PauseFor(Pause);
        public static Unit FromBusinessModel(ClockWorker worker, ClockWorkUnit unit) => new Unit(unit.Start, unit.End) { WorkerHashcode = worker.GetHashCode()}.PauseFor(unit.Pause) as Unit;
    }
}
