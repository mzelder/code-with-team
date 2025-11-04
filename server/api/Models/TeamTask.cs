namespace api.Models
{
    public class TeamTask
    {
        public int Id { get; set; }

        public int TeamTaskProgressId { get; set; }
        public TeamTaskProgress TeamTaskProgress { get; set; }

        public bool IsCompleted { get; set; }
        public string Description { get; set; }
    }
}
