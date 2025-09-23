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
            var categoryExists = await _context.Categories.AnyAsync(c => c.Id == dto.CategoryId);
            var roleExists = await _context.Roles.AnyAsync(r => r.Id == dto.RoleId);
            var validLanguagesIds = await _context.ProgrammingLanguages.Select(p => p.Id).ToListAsync();
            bool allLanguagesValid = dto.ProgrammingLanguageIds.All(id => validLanguagesIds.Contains(id));

            if (!categoryExists || !roleExists || !allLanguagesValid)
            {
                return BadRequest(new { message = "Something went wrong..." });
            }

            int userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
            //var user = await _context.Users.SingleAsync(u => u.Id == userId);

            // Add options that user choose and add row into user selection
            var userSelection = new UserSelection
            {
                UserId = userId,
                CategoryId = dto.CategoryId,
                RoleId = dto.RoleId
            };
            _context.Add(userSelection);

            // Add all choosen programming languages by user
            foreach (var languageId in dto.ProgrammingLanguageIds)
            {
                _context.Add(new UserLanguage
                {
                    UserSelection = userSelection,
                    ProgrammingLanguageId = languageId
                }); 
            };

            // Add user into queue
            _context.Add(new LobbyQueue
            {
                UserId = userId,
                UserSelection = userSelection,
                Status = "Queued"
            });

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
