using api.Models.Tasks;

namespace api.Dtos.TaskProgress
{
    public class TaskProgressDto
    {
        public string Name { get; set; }
        public bool IsCompleted { get; set; }
        public string Description { get; set; }

        public TaskProgressDto(UserTask task)
        {
            Name = task.Name;
            IsCompleted = task.IsCompleted;
            Description = task.Description;
        }

        public TaskProgressDto(TeamTask task)
        {
            Name = task.Name;
            IsCompleted = task.IsCompleted;
            Description = task.Description;
        }
    }   
}
