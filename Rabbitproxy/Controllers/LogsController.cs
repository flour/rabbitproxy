using Microsoft.AspNetCore.Mvc;
using Rabbitproxy.Services;
using Shared;
using System.Threading.Tasks;

namespace Rabbitproxy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private readonly IMessageBus _messageBus;
        private readonly LogStorage _logs;

        public LogsController(IMessageBus messageBus, LogStorage logs)
        {
            _messageBus = messageBus;
            _logs = logs;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllLogs()
        {
            await Task.Delay(100);
            return Ok(_logs.Logs);
        }

        [HttpPost]
        public async Task<IActionResult> AddLog([FromBody] LogMessage log)
        {
            try
            {
                var result = await _messageBus.HandleMessageAsync(log);
                _logs.Logs.Add(log);
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
    }
}