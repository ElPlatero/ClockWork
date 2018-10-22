using System;
using Clockwork.Lib.Models;

namespace Clockwork.Lib.Repositories
{
    public interface IClockWorkRepository
    {
        ClockWorkUnitCollection LoadCalendar(int workerId, DateTime firstDay, DateTime lastDay);
        ClockWorker LoadWorker(int workerId);

        void Save(ClockWorkUnitCollection calendar);
        void Save(ClockWorker worker);
    }
}
