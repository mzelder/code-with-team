namespace api.Models.Tasks
{
    public interface ITask
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsCompleted { get; set; }
        public string Description { get; set; }
    }
}
