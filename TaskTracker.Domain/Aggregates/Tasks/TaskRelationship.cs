namespace TaskTracker.Domain.Aggregates.Tasks
{
    public class TaskRelationship
    {
        protected TaskRelationship() { }

        public TaskRelationship(int relationId, int sourceTaskId, int targetTaskId)
        {
            RelationId = relationId;
            SourceTaskId = sourceTaskId;
            TargetTaskId = targetTaskId;
        }

        public int RelationId { get; private set; }
        public int SourceTaskId { get; private set; }
        public int TargetTaskId { get; private set; }
        public TaskRelationEntity? Relation { get; private set; }
        public TaskEntity? SourceTask { get; private set; }
        public TaskEntity? TargetTask { get; private set; }
    }
}