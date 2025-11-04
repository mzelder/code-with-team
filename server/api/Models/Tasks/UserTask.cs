namespace api.Models.Tasks
{
    public class UserTask : ITask
    {
        public int Id { get; set; }

        public int UserTaskProgressId { get; set; }
        public UserTaskProgress UserTaskProgress { get; set; }

        public string Name { get; set; }
        public bool IsCompleted { get; set; }
        public string Description { get; set; }
    }
}
