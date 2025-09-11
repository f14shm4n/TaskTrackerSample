namespace TaskTracker.Domain.Aggregates.WorkAssignment
{
    public class WorkAssignmentRelationship
    {
        protected WorkAssignmentRelationship() { }

        public WorkAssignmentRelationship(int relationId, int sourceWorkAssignmentId, int targetWorkAssignmentId)
        {
            RelationId = relationId;
            SourceWorkAssignmentId = sourceWorkAssignmentId;
            TargetWorkAssignmentId = targetWorkAssignmentId;
        }

        public int RelationId { get; private set; }
        public int SourceWorkAssignmentId { get; private set; }
        public int TargetWorkAssignmentId { get; private set; }
        public WorkAssignmentRelation? Relation { get; private set; }
        public WorkAssignment? SourceWorkAssignment { get; private set; }
        public WorkAssignment? TargetWorkAssignment { get; private set; }
    }
}