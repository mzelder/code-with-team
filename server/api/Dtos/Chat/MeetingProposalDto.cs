using api.Models;

namespace api.Dtos.Chat
{
    public class MeetingProposalDto
    {
        public string Username { get; set; } // who starts the proposal
        public string MeetingTime { get; set; }
        public string CreatedAt { get; set; }
        public MeetingVoteDto[] Votes { get; set; }
    }
}
