using TaskTracker.Domain.Aggregates.Tasks;

namespace TaskTracker.API.Application.Dto
{
    public class TaskDTO
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public TaskEntityStatus Status { get; set; }
        public TaskEntityPriority Priority { get; set; }
        public string Author { get; set; } = string.Empty;
        public string? Worker { get; set; }
    }
}
