using System;
using Clockwork.Lib.Models;
using Xunit;

namespace ClockWork.Lib.Tests
{
    public class ClockWorkUnitTests
    {
        [Fact]
        public void BasicTest()
        {
            var today = DateTime.Today;
            var now = DateTime.Now;
            var pause = TimeSpan.FromMinutes(30);

            var unit = new ClockWorkUnit(today, now).PauseFor(30);

            Assert.Equal(today, unit.Start);
            Assert.Equal(now, unit.End);
            Assert.Equal(pause, unit.Pause);

            var otherUnit = new ClockWorkUnit(today, now).PauseFor(TimeSpan.FromMinutes(30));

            Assert.Equal(unit, otherUnit);
            Assert.Equal(unit.GetHashCode(), otherUnit.GetHashCode());
        }
    }
}
