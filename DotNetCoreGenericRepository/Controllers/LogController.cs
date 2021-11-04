using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Repository.Interfaces;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCoreGenericRepository.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LogController : ControllerBase
    {
    
        private readonly ILogRepository _logger;
        public LogController(ILogRepository logRepository)
        {
            _logger = logRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFromAttributesAsync()
        {
            var result = await _logger.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("manual")]
        public async Task<IActionResult> GetAllManualAsync()
        {
            var result = await _logger.GetLogsManualProcedureAsync();
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetOneFromAttributesAsync(int id)
        {
            var result = await _logger.GetOneAsync(new Log { LogId=id});
            return Ok(result);
        }

        [HttpPost("manual")]
        public async Task<IActionResult> CreateManualAsync()
        {
            var result = await _logger.InsertLogManualProcedureAsync(new Log { Level = "Level 1", Action = nameof(LogController) });
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateFromAttributesAsync()
        {
            var result = await _logger.InsertAsync(new Log { Level="Level 1",Action=nameof(LogController)});
            return Ok(result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateFromAttributesAsync(int id)
        {
            var result = await _logger.UpdateAsync(new Log {LogId=id, Level = "Level 2", Action = nameof(LogController) });
            return Ok(result);
        }

        [HttpPut("status/{id:int}")]
        public async Task<IActionResult> ChangeStatusFromAttributesAsync(int id)
        {
            var result = await _logger.ChangeStatusAsync(new Log { LogId = id, StatusId = 2 });
            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteFromAttributesAsync(int id)
        {
            var result = await _logger.DeleteAsync(id,"");
            return Ok(result);
        }
    }
}
