namespace api.Models
{
    public class LobbyMember
    {
        public int Id { get; set; }
        public int LobbyId { get; set; }
        public Lobby Lobby { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int UserSelectionId { get; set; }
        public UserSelection UserSelection { get; set; }
        public DateTime JoinedAt { get; set; }
    }
}
