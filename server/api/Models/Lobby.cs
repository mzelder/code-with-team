using api.Models.Tasks;
using api.Models;

namespace api.Models
{
    public enum LobbyStatus
    {
        SchedulingMeeting,
        Working,
        Finished
    }
    
    public class Lobby
    {
        public int Id { get; set; }
        public LobbyStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? RepositoryUrl { get; set; }
        public int? MentorId { get; set; }
        public Mentor.Mentor? Mentor { get; set; }
        public TeamTaskProgress TeamTaskProgress { get; set; }
    }
}
