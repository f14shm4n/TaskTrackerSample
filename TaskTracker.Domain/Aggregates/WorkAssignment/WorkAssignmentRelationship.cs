namespace TaskTracker.Domain.Aggregates.WorkAssignment
{
    public class WorkAssignmentRelationship
    {
        protected WorkAssignmentRelationship() { }

        public WorkAssignmentRelationship(WorkAssignmentRelationType relation, int sourceWorkAssignmentId, int targetWorkAssignmentId)
        {
            Relation = relation;
            SourceWorkAssignmentId = sourceWorkAssignmentId;
            TargetWorkAssignmentId = targetWorkAssignmentId;
        }

        public WorkAssignmentRelationType Relation { get; private set; }
        public int SourceWorkAssignmentId { get; private set; }
        public int TargetWorkAssignmentId { get; private set; }
        public WorkAssignment? SourceWorkAssignment { get; private set; }
        public WorkAssignment? TargetWorkAssignment { get; private set; }
    }
}