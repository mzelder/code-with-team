namespace api.Models.Mentors
{
    public class MentorReview
    {
        public int Id { get; set; }
        
        public int LobbyId { get; set; }
        public Lobby Lobby { get; set; }

        public int MentorId { get; set; }
        public Mentor.Mentor Mentor { get; set; }

        public string? Feedback { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
