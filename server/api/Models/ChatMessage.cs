using System.ComponentModel.DataAnnotations;

namespace api.Models
{
    public class ChatMessage
    {
        public int Id { get; set; }
        
        [Required] public int LobbyId { get; set; }
        public Lobby Lobby { get; set; }

        [Required] public int UserId { get; set; }
        public User User { get; set; }

        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
