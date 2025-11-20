namespace api.Models.Mentor
{
    public class Mentor
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int? MentorFormId { get; set; }
        public MentorForm? MentorForm { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
