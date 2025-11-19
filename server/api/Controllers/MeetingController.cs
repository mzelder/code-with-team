using api.Data;
using api.Dtos;
using api.Services.Interfaces;
using api.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    namespace api.Controllers
    {
        [Route("api/[controller]")]
        [ApiController]
        public class MeetingController : BaseAuthorizedController
        {
            private readonly AppDbContext _context;

            public MeetingController(AppDbContext context)
            {
                _context = context;
            }

            [HttpGet("get-scheduled")]
            public async Task<ActionResult<ScheduledMeetingDto>> GetScheduledMeetings()
            {
                var userLobby = await _context.LobbyMembers
                    .Where(lm => lm.UserId == GetCurrentUserId())
                    .Select(lm => lm.LobbyId)
                    .FirstOrDefaultAsync();

                var cutoff = DateTime.UtcNow.AddMinutes(-30);
                var scheduledMeetingDateTime = await _context.MeetingProposals
                    .Where(mp => mp.MeetingTime >= cutoff)
                    .Where(mp => mp.LobbyId == userLobby)
                    .Select(mp => mp.MeetingTime.ToString())
                    .FirstOrDefaultAsync();

                return Ok(new ScheduledMeetingDto { ScheduledDateTime = scheduledMeetingDateTime });
            }

            [HttpGet("get-link")]
            public async Task<ActionResult<MeetingLinkDto>> GetMeetingLink()
            {
                var userLobby = await _context.LobbyMembers
                    .Where(lm => lm.UserId == GetCurrentUserId())
                    .Select(lm => lm.LobbyId)
                    .FirstOrDefaultAsync();

                var meetingLink = await _context.MeetingProposals
                    .Where(mp => mp.LobbyId == userLobby)
                    .Select(mp => mp.Meeting.TeamsMeetingLink)
                    .FirstOrDefaultAsync();

                return Ok(new MeetingLinkDto { MeetingLink = meetingLink });
            }
        }
    }
}
