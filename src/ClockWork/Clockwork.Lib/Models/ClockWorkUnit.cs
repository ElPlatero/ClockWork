﻿using System;

namespace Clockwork.Lib.Models
{
    public class ClockWorkUnit
    {
        public ClockWorkUnit(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
            Pause = TimeSpan.Zero;
        }

        public ClockWorkUnit PauseFor(int minutes) => PauseFor(TimeSpan.FromMinutes(minutes));
        public ClockWorkUnit PauseFor(TimeSpan pause)
        {
            Pause = Pause.Add(pause);
            return this;
        }

        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public TimeSpan Pause { get; set; }

        public override string ToString()
        {
            return $"{Start:dd.MM.yyyy HH:mm} - {End:dd.MM.yyyy HH:mm} (Pause: {(int)Pause.TotalHours:D2}:{Pause.Minutes:D2})";
        }
    }
}