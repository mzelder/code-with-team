using Microsoft.Graph.Models;

namespace api.Models.Mentors
{
    public class AiSummary
    {
        public int Id { get; set; }

        public int LobbyId { get; set; }
        public Lobby Lobby { get; set; }

        public string SummaryText { get; set; }
        public DateTime GeneratedAt { get; set; }
    }
}
