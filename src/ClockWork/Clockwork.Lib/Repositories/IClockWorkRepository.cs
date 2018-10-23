using System;
using System.Collections.Generic;
using Clockwork.Lib.Models;

namespace Clockwork.Lib.Repositories
{
    public interface IClockWorkRepository
    {
        ClockWorkUnitCollection LoadCalendar(int workerId, DateTime firstDay, DateTime lastDay);
        ClockWorker LoadWorker(int workerId);
        ClockWorker[] LoadWorkers();

        void Save(ClockWorkUnitCollection calendar);
        void Save(ClockWorker worker);
    }
}
