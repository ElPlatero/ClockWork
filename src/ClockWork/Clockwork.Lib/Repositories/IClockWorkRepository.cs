using System;
using Clockwork.Lib.Models;

namespace Clockwork.Lib.Repositories
{
    public interface IClockWorkRepository
    {
        ClockWorkUnitCollection LoadCalendar(ClockWorker worker, DateTime firstDay, DateTime lastDay);
        ClockWorker LoadWorker(string familyName, string givenName, DateTime dateOfBirth);

        void Save(ClockWorkUnitCollection calendar);
        void Save(ClockWorker worker);
    }
}
