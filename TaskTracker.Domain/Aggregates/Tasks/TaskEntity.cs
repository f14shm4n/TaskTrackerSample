using TaskTracker.Domain.Abstractions;

namespace TaskTracker.Domain.Aggregates.Tasks
{
    public class TaskEntity : Entity, IAggregateRoot
    {
        private readonly List<TaskEntity> _subTasks = [];
        private readonly List<TaskRelationship> _outRelations = [];
        private readonly List<TaskRelationship> _inRelations = [];

        protected TaskEntity()
        {
        }

        public TaskEntity(string? title, TaskEntityStatus status, TaskEntityPriority priority, string author, string? worker = null)
        {
            Title = title;
            Status = status;
            Priority = priority;
            Author = author;
            Worker = worker;
        }

        public string? Title { get; private set; }
        public TaskEntityStatus Status { get; private set; }
        public TaskEntityPriority Priority { get; private set; }
        public string Author { get; private set; } = string.Empty; // Simplificated field
        public string? Worker { get; private set; } // Simplificated field
        public int? MasterTaskId { get; private set; }
        public TaskEntity? MasterTask { get; private set; }
        public IReadOnlyCollection<TaskEntity> SubTasks => _subTasks;
        public IReadOnlyCollection<TaskRelationship> OutRelations => _outRelations;
        public IReadOnlyCollection<TaskRelationship> InRelations => _inRelations;

        public void SetStatus(TaskEntityStatus status) => Status = status;

        public void SetPriority(TaskEntityPriority priority) => Priority = priority;

        public void SetAuthor(string author)
        {
            if (author is null || author.Length == 0)
            {
                // May be should throw error
                return;
            }

            Author = author;
        }

        public void SetWorker(string worker)
        {
            if (worker is null || worker.Length == 0)
            {
                return;
            }
            Worker = worker;
        }

        public void ClearWorker() => Worker = null;
    }
}
