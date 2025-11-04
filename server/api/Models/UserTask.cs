namespace api.Models
{
    public class UserTask
    {
        public int Id { get; set; }

        public int UserTaskProgressId { get; set; }
        public UserTaskProgress UserTaskProgress { get; set; }

        public bool IsCompleted { get; set; }
        public string Description { get; set; }
    }
}
