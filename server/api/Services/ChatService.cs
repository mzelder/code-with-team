using api.Data;
using api.Dtos.Chat;
using api.Models;
using api.Services.Interfaces;
using api.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Services
{
    public class ChatService : IChatService
    {
        private readonly AppDbContext _context;
        private readonly IMeetingTimeHelper timeHelper;

        public ChatService(AppDbContext context, IMeetingTimeHelper timeHelper)
        {
            _context = context;
            this.timeHelper = timeHelper;
        }

        public bool IsValidMeetingTime(string proposedTime)
        {
            return timeHelper.IsValidMeetingTime(proposedTime);
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
                    CreatedAt = m.CreatedAt.ToString("O")   
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
                    Id = mp.Id,
                    Username = mp.User.Username,
                    MeetingTime = mp.MeetingTime.ToString("O"),
                    Status = mp.Status,
                    CreatedAt = mp.CreatedAt.ToString("O"),
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
                CreatedAt = DateTime.UtcNow.ToString("O")
            };
        }

        public async Task<MeetingProposalDto> SaveMeetingProposalAsync(CreateMeetingProposalDto proposalDto)
        {
            if (!timeHelper.IsValidMeetingTime(proposalDto.MeetingTime))
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

            var meetingTimeDate = timeHelper.ParseMeetingTime(proposalDto.MeetingTime);
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
                Id = meetingProposal.Id,
                Username = meetingProposal.User.Username,
                MeetingTime = meetingProposal.MeetingTime.ToString("O"),
                CreatedAt = DateTime.UtcNow.ToString("O"),
                Status = MeetingProposalStatus.Pending,
                Votes = [new() {
                    Username = meetingProposal.User.Username,
                    IsAccepted = true
                }]
            };
        }

        public async Task<MeetingProposalDto> SaveMeetingVoteAsync(MeetingVoteDto voteDto, int proposalId)
        {
            var proposal = await _context.MeetingProposals
                .Where(mp => mp.Id == proposalId)
                .FirstOrDefaultAsync()
                ?? throw new Exception("Meeting proposal does not exist");

            var user = await _context.Users.
                FirstOrDefaultAsync(u => u.Username == voteDto.Username)
                ?? throw new Exception("User is not existing");

            var lobby = GetLobbyByUserId(user.Id);

            if (proposal.LobbyId != lobby.Id)
            {
                throw new UnauthorizedAccessException("Meeting proposal does not belong to user's lobby");
            }

            var hasVoted = await _context.MeetingVotes
                .AnyAsync(v => v.MeetingProposalId == proposalId && v.UserId == user.Id);   
            if (hasVoted)
            {
                throw new Exception("User already voted on this proposal");
            }

            var vote = new MeetingVote
            {
                MeetingProposal = proposal,
                User = user,
                Vote = voteDto.IsAccepted,
                VotedAt = DateTime.UtcNow
            };
            _context.MeetingVotes.Add(vote);

            if (!voteDto.IsAccepted)
            {
                proposal.Status = MeetingProposalStatus.Rejected;
            } else
            {
                var positiveVotesCount = await _context.MeetingVotes
                    .CountAsync(v => v.MeetingProposalId == proposalId && v.Vote)
                    + 1;

                if (positiveVotesCount == 4)
                {
                    proposal.Status = MeetingProposalStatus.Accepted;
                }
            }

            await _context.SaveChangesAsync();

            var votes = await _context.MeetingVotes
                .Where(v => v.MeetingProposalId == proposalId)
                .Select(v => new MeetingVoteDto
                {
                    Username = v.User.Username,
                    IsAccepted = v.Vote
                })
                .ToArrayAsync();

            return new MeetingProposalDto
            {
                Id = proposal.Id,
                Username = user.Username,
                MeetingTime = proposal.MeetingTime.ToString("O"),
                CreatedAt = proposal.CreatedAt.ToString("O"),
                Status = proposal.Status,
                Votes = votes
            };
        }
    }
}