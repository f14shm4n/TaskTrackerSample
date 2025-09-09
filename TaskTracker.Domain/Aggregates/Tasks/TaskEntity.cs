using TaskTracker.Domain.Abstractions;

namespace TaskTracker.Domain.Aggregates.Tasks
{
    public class TaskEntity : Entity
    {
        protected TaskEntity()
        {
        }

        public TaskEntity(string? description, TaskEntityStatus status, TaskEntityPriority priority, UserInfo? author, UserInfo? worker = null)
        {
            Description = description;
            Status = status;
            Priority = priority;
            Author = author;
            Worker = worker;
        }

        public string? Description { get; private set; }
        public TaskEntityStatus Status { get; private set; }
        public TaskEntityPriority Priority { get; private set; }
        public UserInfo? Author { get; private set; }
        public UserInfo? Worker { get; private set; }
    }
}
