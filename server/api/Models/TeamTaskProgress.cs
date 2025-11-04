namespace api.Models
{
    public class TeamTaskProgress
    {
        public int Id { get; set; }
        
        public int LobbyId { get; set; }
        public Lobby Lobby { get; set; }
    }
}
