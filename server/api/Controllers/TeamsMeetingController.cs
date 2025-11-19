using api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/teams/meetings")]
    public class TeamsMeetingController : ControllerBase
    {
        private readonly ITeamsMeetingService _teamsMeetingService;

        public TeamsMeetingController(ITeamsMeetingService teamsMeetingService)
        {
            _teamsMeetingService = teamsMeetingService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create()
        {
            try
            {
                var date = DateTime.UtcNow.AddMinutes(15);
                var result = await _teamsMeetingService.CreateOnlineMeetingAsync(date);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}
