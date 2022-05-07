using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FireAndForgetTasks.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly BackgroundWorkerQueue _backgroundWorkerQueue;

        public HomeController(ILogger<HomeController> logger, BackgroundWorkerQueue backgroundWorkerQueue)
        {
            _logger = logger;
            _backgroundWorkerQueue = backgroundWorkerQueue;
        }

        [HttpGet]
        [Route("CallSlowApi")]
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation($"Starting at {DateTime.UtcNow.TimeOfDay}");
            await Task.Delay(10000);
            _logger.LogInformation($"Done at {DateTime.UtcNow.TimeOfDay}");

            return Ok(new
            {
                status = "success"
            });
        }

        [HttpGet]
        [Route("CallSlowApiWithBackgroundProcess")]
        public IActionResult CallSlowApiWithBackgroundProcess()
        {
            _logger.LogInformation($"Starting at {DateTime.UtcNow.TimeOfDay}");
            _backgroundWorkerQueue.QueueBackgroundWorkItem(async token =>
            {
                await Task.Delay(10000);
                _logger.LogInformation($"Done at {DateTime.UtcNow.TimeOfDay}");
            });

            return Ok(new
            {
                status = "success"
            });
        }
    }
}
