namespace api.Models.Tasks
{
    public class UserTaskProgress
    {
        public int Id { get; set; }
        
        public int LobbyMemberId { get; set; }
        public LobbyMember LobbyMember { get; set; }

        public ICollection<UserTask> UserTasks { get; set; }
    }
}
