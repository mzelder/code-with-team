using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.ComponentModel.DataAnnotations;

namespace api.Models
{
    public enum MeetingProposalStatus
    {
        Pending,
        Accepted,
        Rejected
    }

    public class MeetingProposal
    {
        public int Id { get; set; }

        [Required] public int LobbyId { get; set; }
        public Lobby Lobby { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public DateTime MeetingTime { get; set; }
        public DateTime CreatedAt { get; set; }

        public MeetingProposalStatus Status { get; set; }

        public ICollection<MeetingVote> Votes { get; set; }
    }
}
