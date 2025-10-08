using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api.Data;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using api.Dtos.Matchmaking;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using api.Services.Interfaces;
using api.Dtos;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatchmakingController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMatchmakingService _matchmakingService;

        public MatchmakingController(AppDbContext context, IMatchmakingService matchmakingService)
        {
            _context = context;
            _matchmakingService = matchmakingService;
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                throw new UnauthorizedAccessException("User ID not found in claims");
            }

            return int.Parse(userIdClaim.Value);
        }

        [HttpGet("get-options")]
        [Authorize]
        public async Task<ActionResult<MatchmakingResponseDto>> GetOptions()
        {
            var categories = await _context.Categories
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToListAsync();

            var roles = await _context.Roles
                .Select(r => new RoleDto
                {
                    Id = r.Id,
                    Name = r.Name,
                    CategoryId = r.CategoryId
                })
                .ToListAsync();

            var programmingLanguages = await _context.ProgrammingLanguages
                .Select(p => new ProgrammingLanguageDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    RoleId = p.RoleId
                })
                .ToListAsync();

            var result = new MatchmakingResponseDto
            {
                Categories = categories,
                Roles = roles,
                ProgrammingLanguages = programmingLanguages
            };

            return Ok(result);
        }

        [HttpGet("get-choosed-options")]
        [Authorize]
        public async Task<ActionResult<ChoosedOptionsDto>> GetChoosedOptions()
        {
            var response = await _matchmakingService.GetChoosedOptionsAsync(GetCurrentUserId());

            return Ok(response);
        }

        [HttpPost("start-queue")]
        [Authorize]
        public async Task<IActionResult> StartQueue([FromBody] ChoosedOptionsDto dto)
        {
            var response = await _matchmakingService.StartQueueAsync(GetCurrentUserId(), dto);
            
            if (!response.Success)
            {
                return BadRequest(response);
            } 

            return Ok(response);
        }

        [HttpDelete("stop-queue")]
        [Authorize]
        public async Task<ActionResult<ApiResponseDto>> StopQueue()
        {
            var response = await _matchmakingService.StopQueueAsync(GetCurrentUserId());

            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }

        [HttpGet("queue-time")]
        [Authorize]
        public async Task<ActionResult<QueueTimeDto>> GetTimeInQueue()
        {
            var response = await _matchmakingService.GetQueueTimeAsync(GetCurrentUserId());
            
            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }

        [HttpGet("wait-for-lobby")]
        [Authorize]
        public async Task<ActionResult<LobbyStatusDto>> WaitForLobby()
        {
            LobbyStatusDto? lobbyStatus = null;
            const int timeoutSeconds = 20;
            const int pollIntervalSeconds = 10;
            var startTime = DateTime.UtcNow;

            while ((DateTime.UtcNow - startTime).TotalSeconds < timeoutSeconds)
            {
                lobbyStatus = await _matchmakingService.TryGetLobbyAsync();

                if (lobbyStatus != null && lobbyStatus.Found)
                {
                    return Ok(lobbyStatus);
                }

                await Task.Delay(pollIntervalSeconds * 1000);
            }

            return StatusCode(408, lobbyStatus);
        }
    }
}
