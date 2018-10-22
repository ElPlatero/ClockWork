using System;
using Clockwork.Lib.Models;

namespace ClockWork.Data.Models
{
    class Unit
    {
        public int Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public int PauseSeconds { get; set; }
        public Worker Worker { get; set; }

        public ClockWorkUnit ToBusinessModel() => new ClockWorkUnit(Start, End).PauseFor(TimeSpan.FromSeconds(PauseSeconds));

        public static Unit FromBusinessModel(ClockWorkUnit unit, Worker worker) => new Unit
        {
            Worker = worker,
            Start = unit.Start,
            End = unit.End,
            PauseSeconds = Convert.ToInt32(unit.Pause.TotalSeconds)
        };
    }
}