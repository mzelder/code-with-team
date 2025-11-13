using api.Models;
using System.ComponentModel.DataAnnotations;

namespace api.Dtos.Chat
{
    public class MeetingProposalDto
    {
        [Required]
        public int Id { get; set; }
        public string Username { get; set; } // who starts the proposal
        public string MeetingTime { get; set; }
        public string CreatedAt { get; set; }
        public MeetingProposalStatus Status { get; set; }
        public MeetingVoteDto[] Votes { get; set; }
    }
}
