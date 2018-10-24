using System;
using Clockwork.Lib.Models;

namespace ClockWork.Data.Models
{
    internal class Worker
    {
        public int Id { get; set; }
        public string FamilyName { get; set; }
        public string GivenName { get; set; }
        public DateTime DateOfBirth { get; set; }

        public ClockWorker ToBusinessModel() => new ClockWorker(FamilyName, GivenName, DateOfBirth) { Id = Id };

        public static Worker FromBusinessModel(ClockWorker clockWorker) => new Worker
        {
            Id = clockWorker.Id,
            FamilyName = clockWorker.FamilyName,
            GivenName = clockWorker.GivenName,
            DateOfBirth = clockWorker.DateOfBirth
        };
    }
}