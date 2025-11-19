using Azure.Identity;

namespace api.Dtos.Chat
{
    public class CreateMeetingProposalDto
    {
        public string Username { get; set; }
        public string MeetingTime { get; set; }
    }
}
