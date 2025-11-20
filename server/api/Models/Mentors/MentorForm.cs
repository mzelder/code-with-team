namespace api.Models.Mentor
{
    public enum MentorFormStatus
    {
        Pending,
        Approved,
        Rejected
    }

    public class MentorForm
    {
        public int Id { get; set; }
        
        public int UserId { get; set; }
        public User User { get; set; }

        public string FullName { get; set; }
        public string Email { get; set; }

        public string Motivation { get; set; }
        public DateTime CreatedAt { get; set; }
        public MentorFormStatus Status { get; set; }

        public List<MentorAssesmentAnswer> MentorAssesmentAnswers { get; set; }
        public List<MentorPortfolioLink> MentorPortfolioLinks { get; set; }
    }
}
