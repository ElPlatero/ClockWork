using System;
using System.Collections.Generic;
using System.Linq;

namespace Clockwork.Lib.Calculators
{
    public class CalculationResultCollection : List<CalculationResult>
    {
        public CalculationResultCollection(IEnumerable<CalculationResult> results = null)
        {
            if (results != null)
            {
                AddRange(
                    results
                        .GroupBy(p => p.Date)
                        .Select(p => new CalculationResult(
                            p.Key,
                            p.First().WorkingHours,
                            TimeSpan.FromSeconds(p.Sum(q => q.ExactWorkedHours.TotalSeconds)),
                            TimeSpan.FromSeconds(p.Sum(q => q.CalculatedWorkedHours.TotalSeconds)))));
            }
        }

        public TimeSpan WorkingHours => TimeSpan.FromSeconds(this.Sum(p => p.WorkingHours.TotalSeconds));
        public TimeSpan ExactWorkedHours => TimeSpan.FromSeconds(this.Sum(p => p.ExactWorkedHours.TotalSeconds));
        public TimeSpan CalculatedWorkedHours => TimeSpan.FromSeconds(this.Sum(p => p.CalculatedWorkedHours.TotalSeconds));
        public TimeSpan Overtime => CalculatedWorkedHours - WorkingHours;

        public CalculationGrouping GroupBy => new CalculationGrouping(this);
    }
}