using Clockwork.Lib.Models;

namespace Clockwork.Lib.Repositories
{
    public interface IClockWorkRepository
    {
        ClockWorkUnitCollection LoadCalendar(int workerId);
        ClockWorker LoadWorker(int workerId);
        ClockWorker[] LoadWorkers();

        void Save(ClockWorkUnitCollection calendar);
        void Save(ClockWorker worker);
    }
}
