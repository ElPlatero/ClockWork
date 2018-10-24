using System;
using System.Linq;
using System.Threading.Tasks;
using Clockwork.Lib.Calculators;
using Clockwork.Lib.Models;
using Clockwork.Lib.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ClockWork.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkerController : ControllerBase
    {
        private readonly IClockWorkRepository _repository;
        private readonly IEffectiveWorkingTimeCalculator _calculator;

        public WorkerController(IClockWorkRepository repository, IEffectiveWorkingTimeCalculator calculator)
        {
            _repository = repository;
            _calculator = calculator;
        }

        [HttpGet]
        public Task<IActionResult> GetWorkers()
        {
            return Task.FromResult((IActionResult)Ok(_repository.LoadWorkers().Select(p => new
            {
                p.Id,
                Name = $"{p.GivenName} {p.FamilyName}"
            })));
        }

        [HttpGet("{id}")]
        public Task<IActionResult> GetWorker(int id)
        {
            var worker = _repository.LoadWorker(id);
            var result = worker == null ? (IActionResult) NotFound() : Ok(worker);

            return Task.FromResult(result);
        }

        [HttpPost]
        public Task<IActionResult> CreateWorker([FromBody] ClockWorker worker)
        {
            worker.Id = 0;
            _repository.Save(worker);
            return Task.FromResult((IActionResult)Ok(worker));
        }

        [HttpPut("{id}")]
        public Task<IActionResult> UpdateWorker(int id, [FromBody] ClockWorker worker)
        {
            if (worker.Id != id) worker.Id = id;

            _repository.Save(worker);

            IActionResult result = Ok(worker);
            return Task.FromResult(result);
        }

        //Adds a unit of work to the existing calendar, with regard to existing unit. Overlapping won't occur.
        [HttpPut("{id}/unit")]
        public Task<IActionResult> AddUnitOfWork(int id, [FromBody] ClockWorkUnit unit)
        {
            var calender = _repository.LoadCalendar(id);
            if (calender == null)
            {
                var worker = _repository.LoadWorker(id);
                if (worker == null) return Task.FromResult((IActionResult)NotFound());

                calender = new ClockWorkUnitCollection(_repository.LoadWorker(id));
            }

            calender.Add(unit);
            _repository.Save(calender);

            return Task.FromResult((IActionResult)Ok());
        }

        //Removes a unit of work from an existing calendar, effectively shrinking existing units or removing them completely.
        [HttpDelete("{id}/unit")]
        public IActionResult RemoveUnitOfWork(int id, [FromBody] ClockWorkUnit unit)
        {
            var calender = _repository.LoadCalendar(id);
            if (calender == null) return NotFound();

            calender.Remove(unit);
            _repository.Save(calender);

            return Ok();
        }

[HttpGet("{id}/result/daily")]
        public IActionResult CalculateWork(int id)
        {
            return Calculate(this, id);
        }

        [HttpGet("{id}/result/weekly")]
        public IActionResult CalculateWeekyWork(int id)
        {
            return Calculate(this, id, p => p.GroupBy.Week);
        }

        [HttpGet("{id}/result/monthly")]
        public IActionResult CalculateMonthlyWork(int id)
        {
            return Calculate(this, id, p => p.GroupBy.Month);
        }

        [HttpGet("{id}/result/yearly")]
        public IActionResult CalculateYearlyWork(int id)
        {
            return Calculate(this, id, p => p.GroupBy.Year);
        }

        private static IActionResult Calculate(WorkerController controller, int workerId, Func<CalculationResultCollection, CalculationResultCollection> group = null)
        {
            var calender = controller._repository.LoadCalendar(workerId);
            if (calender == null) return new NotFoundResult();

            return new OkObjectResult(
                group != null
                ? group(controller._calculator.Calculate(calender))
                : controller._calculator.Calculate(calender)
            );
        }
    }
}