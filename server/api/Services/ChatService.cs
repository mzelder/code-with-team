using api.Data;
using api.Dtos.Chat;
using api.Models;
using api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Services
{
    public class ChatService : IChatService
    {
        private readonly AppDbContext _context;

        public ChatService(AppDbContext context)
        {
            _context = context;
        }

        private Lobby GetLobbyByUserId(int userId)
        {
            var lobby = _context.LobbyMembers
                .Where(lm => lm.UserId == userId)
                .Select(lm => lm.Lobby)
                .FirstOrDefault();
            if (lobby == null)
            {
                throw new InvalidOperationException("User is not part of any lobby.");
            }
            return lobby;
        }

        
        public async Task<IEnumerable<ChatMessageDto>> GetMessagesAsync(int userId)
        {
            var lobby = GetLobbyByUserId(userId);

            var messages = await _context.ChatMessages
                .Where(m => m.LobbyId == lobby.Id)
                .Select(m => new ChatMessageDto
                {
                    Username = m.User.Username,
                    Message = m.Message,
                    CreatedAt = m.CreatedAt.ToString("g")
                })
                .ToListAsync();

            return messages;
        }

        public async Task<IEnumerable<MeetingProposalDto>> GetMeetingProposalsAsync(int userId)
        {
            var lobby = GetLobbyByUserId(userId);

            var proposals = await _context.MeetingProposals
                .Where(mp => mp.LobbyId == lobby.Id)
                .Select(mp => new MeetingProposalDto
                {
                    Username = mp.User.Username,
                    MeetingTime = mp.MeetingTime.ToString("g"),
                    CreatedAt = mp.CreatedAt.ToString("g"),
                    Votes = mp.Votes.Select(v => new MeetingVoteDto
                    {
                        Username = v.User.Username,
                        IsAccepted = v.Vote
                    }).ToArray()
                })
                .ToListAsync();

            return proposals;
        }

        public async Task<ChatMessageDto> SaveMessageAsync(ChatMessageDto messageDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == messageDto.Username);
            if (user == null)
            {
                throw new Exception("User is not existing");
            }

            var lobby = GetLobbyByUserId(user.Id);
            if (lobby == null)
            {
                throw new Exception("Lobby is not existing");
            }

            var chatMessage = new ChatMessage
            {
                Lobby = lobby,
                User = user,
                Message = messageDto.Message,
                CreatedAt = DateTime.UtcNow
            };

            _context.ChatMessages.Add(chatMessage);
            await _context.SaveChangesAsync();

            return new ChatMessageDto
            {
                Username = user.Username,
                Message = messageDto.Message,
                CreatedAt = DateTime.UtcNow.ToString("g")
            };
        }

        public async Task<MeetingProposalDto> SaveMeetingProposalAsync(MeetingProposalDto proposalDto)
        {
            if (!IsValidMeetingTime(proposalDto.MeetingTime))
            {
                throw new Exception("Invalid meeting time format");
            }
            
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == proposalDto.Username);
            if (user == null)
            {
                throw new Exception("User is not existing");
            }

            var lobby = GetLobbyByUserId(user.Id);
            if (lobby == null)
            {
                throw new Exception("Lobby is not existing");
            }

            var meetingTimeDate = ParseMeetingTime(proposalDto.MeetingTime);
            var meetingProposal = new MeetingProposal
            {
                Lobby = lobby,
                User = user,
                MeetingTime = meetingTimeDate,
                CreatedAt = DateTime.UtcNow
            };

            var meetingVote = new MeetingVote
            {
                MeetingProposal = meetingProposal,
                User = user,
                Vote = true,
                VotedAt = DateTime.UtcNow
            };

            _context.MeetingProposals.Add(meetingProposal);
            _context.MeetingVotes.Add(meetingVote);
            await _context.SaveChangesAsync();

            return new MeetingProposalDto
            {
                Username = meetingProposal.User.Username,
                MeetingTime = meetingProposal.MeetingTime.ToString("g"),
                CreatedAt = DateTime.UtcNow.ToString("g"),
                Votes = [new() {
                    Username = meetingProposal.User.Username,
                    IsAccepted = true
                }]
            };
        }

        public bool IsValidMeetingTime(string proposedTime)
        {
            var now = DateTime.UtcNow;
            var proposed = ParseMeetingTime(proposedTime);
            var maxTime = now.AddDays(7);
            return proposed > now && proposed <= maxTime;
        }

        public DateTime ParseMeetingTime(string meetingTimeString)
        {
            // Expected format: MM:DD:hh:mm
            var parts = meetingTimeString.Split(':');

            if (parts.Length != 4)
            {
                throw new FormatException("Invalid meeting time format. Expected MM:DD:hh:mm");
            }

            if (!int.TryParse(parts[0], out int month) ||
                !int.TryParse(parts[1], out int day) ||
                !int.TryParse(parts[2], out int hour) ||
                !int.TryParse(parts[3], out int minute))
            {
                throw new FormatException("Invalid meeting time format. All parts must be numbers.");
            }

            int year = DateTime.UtcNow.Year;

            return new DateTime(year, month, day, hour, minute, 0, DateTimeKind.Utc);
        }
    }
}