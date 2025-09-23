using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api.Data;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using api.Dtos.Matchmaking;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatchmakingController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MatchmakingController(AppDbContext context)
        {
            _context = context;
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

        [HttpPost("start-queue")]
        [Authorize]
        public async Task<IActionResult> StartQueue([FromBody] MatchmakingRequestDto dto)
        {
            var categoryId = dto.CategoryId;
            var roleId = dto.RoleId;
            var programmingLanguageIds = dto.ProgrammingLanguageIds;

            return Ok($"{categoryId}, {roleId}, {programmingLanguageIds}");
        }
    }
}
