using api.Dtos.Chat;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Octokit;

namespace api.Services.Interfaces
{
    public interface IChatService
    {
        Task<IEnumerable<ChatMessageDto>> GetMessagesAsync(int userId);
        Task<IEnumerable<MeetingProposalDto>> GetMeetingProposalsAsync(int userId);
        Task<ChatMessageDto> SaveMessageAsync(ChatMessageDto messageDto);
        Task<MeetingProposalDto> SaveMeetingProposalAsync(MeetingProposalDto proposalDto);
        bool IsValidMeetingTime(string proposedTime);
    }
}
