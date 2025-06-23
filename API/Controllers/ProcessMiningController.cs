using Microsoft.AspNetCore.Mvc;
using API.Data;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProcessMiningController : ControllerBase
    {
        private readonly SQLServerDatabaseContext _context;

        public ProcessMiningController(SQLServerDatabaseContext context)
        {
            _context = context;
        }

        [HttpGet("log")]
        public async Task<IActionResult> GetEventLog()
        {
            var logs = await _context.EventLogs
                .OrderBy(e => e.Timestamp)
                .Select(e => new {
                    CaseId = e.OrderId,
                    Timestamp = e.Timestamp,
                    Activity = e.Activity,
                    Event = e.Details
                }).ToListAsync();

            return Ok(logs);
        }
    }

}
