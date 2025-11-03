using api.Dtos.TaskProgress;
using api.Data;
using api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Microsoft.EntityFrameworkCore;
using api.Dtos;
using api.Dtos.Chat;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : BaseAuthorizedController
    {
        private readonly AppDbContext _context;

        public ChatController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("get-messages")]
        [Authorize]
        public async Task<ActionResult<ChatMessageDto>> GetMessages()
        {
            try
            {
                var lobbyId = await _context.LobbyMembers
                    .Where(lm => lm.UserId == GetCurrentUserId())
                    .Select(lm => lm.LobbyId)
                    .FirstOrDefaultAsync();

                var messages = await _context.ChatMessages
                    .Where(m => m.LobbyId == lobbyId)
                    .Select(m => new ChatMessageDto
                    {
                        Username = m.User.Username,
                        Message = m.Message,
                        Date = m.CreatedAt.ToString("g")
                    })
                    .ToListAsync();

                return Ok(messages);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ApiResponseDto(false, ex.Message));
            }
        }
    }
}
