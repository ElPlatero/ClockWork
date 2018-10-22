using ClockWork.Data.Models;
using LiteDB;

namespace ClockWork.Data
{
    class ClockWorkDatabase : LiteDatabase
    {
        public ClockWorkDatabase(string database) : base(database) { }

        public LiteCollection<Worker> Workers => GetCollection<Worker>("workers");
        public LiteCollection<Unit> Units => GetCollection<Unit>("units");
    }
}
