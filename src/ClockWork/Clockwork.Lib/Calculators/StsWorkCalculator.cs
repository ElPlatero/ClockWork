﻿using System;
using System.Linq;
using Clockwork.Lib.Models;

namespace Clockwork.Lib.Calculators
{
    public class StsWorkCalculator : IEffectiveWorkingTimeCalculator
    {
        private static readonly TimeSpan WorkingHours = TimeSpan.FromHours(8);

        private static CalculationResult Calculate(ClockWorkUnit unit)
        {
            var start = RoundDate(unit.Start, Math.Ceiling);
            var end = RoundDate(unit.End, Math.Floor);

            var pause = unit.Pause;
            var calculatedDuration = end - start;
            if (calculatedDuration <= TimeSpan.Zero) pause = TimeSpan.Zero;
            if (calculatedDuration - pause > TimeSpan.FromHours(6) && pause < TimeSpan.FromMinutes(30)) pause = TimeSpan.FromMinutes(30);
            if (calculatedDuration - pause > TimeSpan.FromHours(9) && pause < TimeSpan.FromMinutes(45)) pause = TimeSpan.FromMinutes(45);

            calculatedDuration -= pause;

            if (calculatedDuration > TimeSpan.FromHours(10)) calculatedDuration = TimeSpan.FromHours(10);
            else if(calculatedDuration < TimeSpan.Zero) calculatedDuration = TimeSpan.Zero;

            return new CalculationResult(unit.Start.Date, WorkingHours, unit.End - unit.Start - pause, calculatedDuration);
        }

        private static DateTime RoundDate(DateTime date, Func<double, double> approximate)
        {
            var minutes = approximate(date.TimeOfDay.TotalMinutes / 10.0) * 10;
            return date.Date.AddMinutes(minutes);
        }

        public CalculationResultCollection Calculate(ClockWorkUnitCollection units)
        {
            return new CalculationResultCollection(units.Select(Calculate));
        }

        public CalculationResult GetBalance(ClockWorkUnitCollection units, DateTime snapshot)
        {
            var result = new CalculationResultCollection(new ClockWorkUnitCollection(units.Worker, units.Where(p => p.End <= snapshot).ToArray()).Select(Calculate));
            return new CalculationResult(units.First().Start.Date, result.WorkingHours, result.ExactWorkedHours, result.CalculatedWorkedHours);
        }
    }
}