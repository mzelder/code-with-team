namespace api.Models.Tasks
{
    public enum TaskCategory
    {
        User,
        Team
    }
    
    public class TaskDefinitions : ITask
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsCompleted { get; set; }
        public string Description { get; set; }
        public TaskCategory Category { get; set; }
    }
}
