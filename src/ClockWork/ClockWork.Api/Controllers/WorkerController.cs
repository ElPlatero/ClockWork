using System.Linq;
using System.Threading.Tasks;
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

        public WorkerController(IClockWorkRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public Task<IActionResult> GetWorkers()
        {
            return Task.FromResult((IActionResult) Ok(_repository.LoadWorkers().Select(p => new
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
            IActionResult result;
            if (worker.Id != id) worker.Id = id;

            _repository.Save(worker);

            result = Ok(worker)
        }

    }
}