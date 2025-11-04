namespace api.Models.Tasks
{
    public class TeamTask : ITask
    {
        public int Id { get; set; }

        public int TeamTaskProgressId { get; set; }
        public TeamTaskProgress TeamTaskProgress { get; set; }

        public string Name { get; set; }
        public bool IsCompleted { get; set; }
        public string Description { get; set; }
    }
}
