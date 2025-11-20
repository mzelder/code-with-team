namespace api.Models.Mentor
{
    public class MentorAssesmentAnswer
    {
        public int Id { get; set; }
        public string AnswerText { get; set; }
        public int MentorQuestionId { get; set; }
        public MentorQuestion MentorQuestion { get; set; }
        public int MentorFormId { get; set; }
        public MentorForm MentorForm { get; set; }
    }
}
