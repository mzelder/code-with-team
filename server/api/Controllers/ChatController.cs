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
using api.Services;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : BaseAuthorizedController
    {
        private readonly AppDbContext _context;
        private readonly IChatService _chatService;

        public ChatController(AppDbContext context, IChatService chatService)
        {
            _context = context;
            _chatService = chatService;
        }

        [HttpGet("get-messages")]
        [Authorize]
        public async Task<ActionResult<ChatMessageDto>> GetMessages()
        {
            try
            {
                var response = await _chatService.GetMessagesAsync(GetCurrentUserId());
                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ApiResponseDto(false, ex.Message));
            }
        }

        [HttpGet("get-meeting-proposals")]
        [Authorize]
        public async Task<ActionResult<ChatMessageDto>> GetMeetingProposals()
        {
            try
            {
                var response = await _chatService.GetMeetingProposalsAsync(GetCurrentUserId());
                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ApiResponseDto(false, ex.Message));
            }
        }
    }
}
