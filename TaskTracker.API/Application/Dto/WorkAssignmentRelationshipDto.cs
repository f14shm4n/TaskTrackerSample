using TaskTracker.Domain.Aggregates.WorkAssignment;

namespace TaskTracker.API.Application.Dto
{
    public class WorkAssignmentRelationshipDto
    {
        public WorkAssignmentRelationType RelationType { get; set; }
        public int SourceId { get; set; }
        public int TargetId { get; set; }
    }
}
