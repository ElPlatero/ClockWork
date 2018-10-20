using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Clockwork.Lib.Models
{
    public class ClockWorkUnitCollection : ICollection<ClockWorkUnit>
    {
        public ClockWorker Worker { get; }
        private readonly HashSet<ClockWorkUnit> _units = new HashSet<ClockWorkUnit>();

        public ClockWorkUnitCollection(ClockWorker worker, params ClockWorkUnit[] units)
        {
            Worker = worker;

            if (units != null)
            {
                foreach (var clockWorkUnit in units)
                {
                    Add(clockWorkUnit);
                }
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
                    if (clockWorkUnit.Start <= item.Start && clockWorkUnit.End <= item.End) clockWorkUnit.End = item.End;
                    else if (clockWorkUnit.Start <= item.Start && clockWorkUnit.End >= item.End) return;
                    else if (clockWorkUnit.Start > item.Start && clockWorkUnit.End >= item.End) clockWorkUnit.Start = item.Start;
                    else if (clockWorkUnit.Start > item.Start && clockWorkUnit.End < item.End)
                    {
                        clockWorkUnit.Start = item.Start;
                        clockWorkUnit.End = item.End;
                    }
                }
            }
        }

        public void Clear() => _units.Clear();
        public bool Contains(ClockWorkUnit item) => _units.Contains(item);
        public void CopyTo(ClockWorkUnit[] array, int arrayIndex) => _units.CopyTo(array, arrayIndex);
        public bool Remove(ClockWorkUnit item) => _units.Remove(item);

        public int Count => _units.Count;
        public bool IsReadOnly => false;
    }
}