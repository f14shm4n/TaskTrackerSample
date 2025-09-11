using TaskTracker.Domain.Abstractions;

namespace TaskTracker.Domain.Aggregates.WorkAssignment
{
    public class WorkAssignmentRelation : Entity
    {
        private readonly List<WorkAssignmentRelationship> _relationships = [];

        public WorkAssignmentRelationType RelationType { get; private set; }

        public IReadOnlyCollection<WorkAssignmentRelationship> Relationships => _relationships;

        public void SetRelationType(WorkAssignmentRelationType relationType) => RelationType = relationType;
    }
}