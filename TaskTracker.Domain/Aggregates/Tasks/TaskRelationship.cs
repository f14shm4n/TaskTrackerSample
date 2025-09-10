namespace TaskTracker.Domain.Aggregates.Tasks
{
    public class TaskRelationship
    {
        protected TaskRelationship() { }

        public TaskRelationship(int relationId, int leftTaskId, int rightTaskId)
        {
            RelationId = relationId;
            LeftTaskId = leftTaskId;
            RightTaskId = rightTaskId;
        }

        public int RelationId { get; private set; }
        public int LeftTaskId { get; private set; }
        public int RightTaskId { get; private set; }
        public TaskRelationEntity? Relation { get; private set; }
        public TaskEntity? LeftTask { get; private set; }
        public TaskEntity? RightTask { get; private set; }
    }
}