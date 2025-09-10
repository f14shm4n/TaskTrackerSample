using TaskTracker.Domain.Abstractions;

namespace TaskTracker.Domain.Aggregates.Tasks
{
    public class TaskEntity : Entity, IAggregateRoot
    {
        private readonly List<TaskEntity> _subTasks = [];
        //private readonly List<TaskRelationship> _relatedTasks = [];

        protected TaskEntity()
        {
        }

        public TaskEntity(string? title, string? description, TaskEntityStatus status, TaskEntityPriority priority, string author, string? worker = null)
        {
            Title = title;
            Description = description;
            Status = status;
            Priority = priority;
            Author = author;
            Worker = worker;
        }

        public string? Title { get; private set; }
        public string? Description { get; private set; } // TODO: Remove on final
        public TaskEntityStatus Status { get; private set; }
        public TaskEntityPriority Priority { get; private set; }
        public string Author { get; private set; } = string.Empty;// Simplificated field
        public string? Worker { get; private set; }// Simplificated field
        public int? MasterTaskId { get; private set; }
        public TaskEntity? MasterTask { get; private set; }
        public IReadOnlyCollection<TaskEntity> SubTasks => _subTasks;
        //public IReadOnlyCollection<TaskRelationship> RelatedTasks => _relatedTasks;
    }
}
