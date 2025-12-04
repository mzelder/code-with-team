using System.ComponentModel.DataAnnotations;
using api.Models.Mentor;

namespace api.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public string? GithubToken { get; set; }

        public DateTime CreatedAt { get; set; }

        public ICollection<UserSelection> UserSelections { get; set; }
        public ICollection<LobbyMember> LobbbyQueues { get; set; }
        public ICollection<MentorForm> MentorForms { get; set; }
        public Mentor.Mentor Mentor { get; set; }
    }
}
