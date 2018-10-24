using System;

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

        public override bool Equals(object obj)
        {
            return obj is ClockWorkUnit unit && Equals(unit);
        }

        protected bool Equals(ClockWorkUnit other)
        {
            return other != null && Equals(Start, other.Start) && Equals(End, other.End) && Equals(Pause, other.Pause);
        }

        private static bool Equals(DateTime one, DateTime other) => Math.Abs((one - other).TotalSeconds) < 1;
        private static bool Equals(TimeSpan one, TimeSpan other) => Math.Abs((one - other).TotalSeconds) < 1;

        private static readonly DateTime BaseTime = new DateTime(2010, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        public override int GetHashCode()
        {
            unchecked
            {
                //ya I know. this hash is not unique over the objects lifetime. It's intended.
                var hashCode = (int)(Start - BaseTime).TotalSeconds;
                hashCode = (hashCode * 397) ^ (int)(End - BaseTime).TotalSeconds;
                hashCode = (hashCode * 397) ^ (int)Pause.TotalSeconds;
                return hashCode;
            }
        }
    }
}