using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Clockwork.Lib.Models;
using Xunit;
using Xunit.Sdk;

namespace ClockWork.Lib.Tests
{
    public class ClockWorkerTests
    {
        [Fact]
        public void EqualityTest()
        {
            var worker = new ClockWorker("Mustermann", "Max", DateTime.Now.AddYears(-21));
            var otherWorker = new ClockWorker("Mustermann", "max", DateTime.Today.AddYears(-21));
            var thirdWorker = new ClockWorker("Schildt", "Hans", new DateTime(1971, 12, 1));

            Assert.Equal(worker, otherWorker);
            Assert.Equal(worker, worker);
            Assert.Equal(worker.Clone(), otherWorker);
            Assert.NotEqual(worker, thirdWorker);

            Assert.Equal(2, new [] {  worker, worker, worker, otherWorker, thirdWorker}.Distinct().Count());
        }
    }
}
