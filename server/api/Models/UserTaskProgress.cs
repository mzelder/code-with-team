namespace api.Models
{
    public class UserTaskProgress
    {
        public int Id { get; set; }

        public int LobbyMemberId { get; set; }
        public LobbyMember LobbyMember { get; set; }

        public bool JoinedVideoCall { get; set; }
        public bool VisitedRepo { get; set; }
        public bool StartedCoding { get; set; }
    }
}
