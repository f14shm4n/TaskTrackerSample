using TaskTracker.Domain.Abstractions;

namespace TaskTracker.Domain.Aggregates.Tasks
{
    public class TaskRelationEntity : Entity
    {
        private readonly List<TaskRelationship> _relationships = [];        

        public IReadOnlyCollection<TaskRelationship> Relationships => _relationships;        
    }
}