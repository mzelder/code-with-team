namespace api.Models.Mentor
{
    public class MentorQuestion
    {
        public int Id { get; set; }
        public string QuestionText { get; set; }
        public ICollection<MentorAssesmentAnswer> MentorAssesmentAnswers { get; set; }
    }
}
