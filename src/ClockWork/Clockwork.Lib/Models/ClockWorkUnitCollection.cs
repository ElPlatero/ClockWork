using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Clockwork.Lib.Models
{
    public class ClockWorkUnitCollection : ICollection<ClockWorkUnit>
    {
        public ClockWorker Worker { get; }
        private IList<ClockWorkUnit> _units = new List<ClockWorkUnit>();

        public ClockWorkUnitCollection(ClockWorker worker, params ClockWorkUnit[] units)
        {
            Worker = worker;

            if (units == null) return;
            foreach (var clockWorkUnit in units)
            {
                Add(clockWorkUnit);
            }
        }

        public IEnumerator<ClockWorkUnit> GetEnumerator() => _units.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Add(ClockWorkUnit item)
        {
            if(item == null) throw new ArgumentNullException(nameof(item));

            var unitsOfInterest = _units.Where(p => p.End >= item.Start && p.Start <= item.End).ToArray();
            if (!unitsOfInterest.Any()) _units.Add(item);
            else
            {
                foreach (var clockWorkUnit in unitsOfInterest)
                {
                    if (clockWorkUnit.Start < item.Start && clockWorkUnit.End < item.End) clockWorkUnit.End = item.End;
                    else if (clockWorkUnit.Start <= item.Start && clockWorkUnit.End >= item.End) return;
                    else if (clockWorkUnit.Start > item.Start && clockWorkUnit.End > item.End) clockWorkUnit.Start = item.Start;
                    else if (clockWorkUnit.Start > item.Start && clockWorkUnit.End < item.End)
                    {
                        clockWorkUnit.Start = item.Start;
                        clockWorkUnit.End = item.End;
                    }

                    while(clockWorkUnit.Start.DayOfYear != clockWorkUnit.End.DayOfYear)
                    {
                        var split = new ClockWorkUnit(clockWorkUnit.End.Date, clockWorkUnit.End);
                        clockWorkUnit.End = split.Start.AddSeconds(-1);
                        _units.Add(split);
                    }
                }

                _units = _units.OrderBy(p => p.Start).ToList();

            }
        }

        public bool Remove(ClockWorkUnit item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            var unitsOfInterest = _units.Where(p => p.End >= item.Start && p.Start <= item.End).ToArray();
            if (!unitsOfInterest.Any()) return false;

            foreach (var clockWorkUnit in unitsOfInterest)
            {
                if (clockWorkUnit.Start < item.Start && clockWorkUnit.End < item.End) clockWorkUnit.End = item.Start;
                else if (clockWorkUnit.Start < item.Start && clockWorkUnit.End > item.End)
                {
                    var newItem = new ClockWorkUnit(item.End, clockWorkUnit.End);
                    clockWorkUnit.End = item.Start;
                    Add(newItem);
                }
                else if (clockWorkUnit.Start > item.Start && clockWorkUnit.End > item.End)
                {
                    clockWorkUnit.Start = item.End;
                }
                else if (clockWorkUnit.Start >= item.Start && clockWorkUnit.End <= item.End)
                {
                    _units.Remove(clockWorkUnit);
                }
            }

            _units = _units.OrderBy(p => p.Start).ToList();
            return true;
        }


        public void Clear() => _units.Clear();
        public bool Contains(ClockWorkUnit item) => _units.Contains(item);
        public void CopyTo(ClockWorkUnit[] array, int arrayIndex) => _units.CopyTo(array, arrayIndex);

        public int Count => _units.Count;
        public bool IsReadOnly => false;
    }
}