using TaskTracker.Domain.Abstractions;

namespace TaskTracker.Domain.Aggregates.WorkAssignment
{
    public class WorkAssignment : Entity, IAggregateRoot
    {
        private readonly List<WorkAssignment> _subAssignment = [];
        private readonly List<WorkAssignmentRelationship> _outRelations = [];
        private readonly List<WorkAssignmentRelationship> _inRelations = [];

        protected WorkAssignment()
        {
        }

        public WorkAssignment(string? title, WorkAssignmentStatus status, WorkAssignmentPriority priority, string author, string? worker = null)
        {
            Title = title;
            Status = status;
            Priority = priority;
            Author = author;
            Worker = worker;
        }

        public string? Title { get; private set; }
        public WorkAssignmentStatus Status { get; private set; }
        public WorkAssignmentPriority Priority { get; private set; }
        public string Author { get; private set; } = string.Empty; // Simplificated field
        public string? Worker { get; private set; } // Simplificated field
        public int? HeadAssignemtId { get; private set; }
        public WorkAssignment? HeadAssignment { get; private set; }
        public IReadOnlyCollection<WorkAssignment> SubAssignment => _subAssignment;
        public IReadOnlyCollection<WorkAssignmentRelationship> OutRelations => _outRelations;
        public IReadOnlyCollection<WorkAssignmentRelationship> InRelations => _inRelations;

        public void SetStatus(WorkAssignmentStatus status) => Status = status;

        public void SetPriority(WorkAssignmentPriority priority) => Priority = priority;

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

        public void SetHeadAssignment(int headAssignmentId)
        {
            if (headAssignmentId <= 0)
            {
                return;
            }
            HeadAssignemtId = headAssignmentId;
        }

        public void RemoveHeadAssignment()
        {
            HeadAssignemtId = null;
        }

        public void AddOutRelation(WorkAssignmentRelationType relationType, int toWorkAssignmentId)
        {
            _outRelations.Add(new WorkAssignmentRelationship(relationType, Id, toWorkAssignmentId));
        }
        public void AddInRelation(WorkAssignmentRelationType relationType, int fromWorkAssignmentId)
        {
            _inRelations.Add(new WorkAssignmentRelationship(relationType, fromWorkAssignmentId, Id));
        }

        public void RemoveOutRelation(WorkAssignmentRelationType relationType, int toWorkAssignmentId)
        {
            var rel = _outRelations.Find(x => x.Relation == relationType && x.TargetWorkAssignmentId == toWorkAssignmentId);
            if (rel is not null)
            {
                _outRelations.Remove(rel);
            }
        }
    }
}
