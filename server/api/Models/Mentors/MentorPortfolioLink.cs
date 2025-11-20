namespace api.Models.Mentor
{
    public class MentorPortfolioLink
    {
        public int Id { get; set; }
        public int MentorFormId { get; set; }
        public MentorForm MentorForm { get; set; }
        public string Url { get; set; }
    }
}
