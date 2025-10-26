using System;
using System.ComponentModel.DataAnnotations;

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
    }
}
