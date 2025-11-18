namespace api.Models.Meetings
{
    public class Meeting
    {
        public int Id { get; set; }

        public int MeetingProposalId { get; set; }
        public MeetingProposal MeetingProposal { get; set; }

        public string TeamsMeetingLink { get; set; }
    }
}
