using System.ComponentModel.DataAnnotations;

namespace api.Dtos.Mentor
{
    public class MentorFormDto
    {
        [Required]
        public string Fullname { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string Motivation { get; set; }
        public string[] PortfolioLinks { get; set; }

        [Required]
        public string[] ScenarioAnswers { get; set; }
    }
}
