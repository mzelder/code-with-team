namespace api.Models.Meetings
{
    public class MeetingVote
    {
        public int Id { get; set; }
        
        public int MeetingProposalId { get; set; }
        public MeetingProposal MeetingProposal { get; set; }
        
        public int UserId { get; set; }
        public User User { get; set; }
        public bool Vote { get; set; }
        public DateTime VotedAt { get; set; }
    }
}
