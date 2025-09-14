using TaskTracker.Domain.Abstractions;

namespace TaskTracker.Domain.Aggregates.WorkAssignment
{
    public interface IWorkAssignmentRepository : IRepository<WorkAssignment>
    {
        WorkAssignment Add(WorkAssignment workAssignment);
        Task<WorkAssignment?> GetAsync(int id, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
        Task<bool> ContainsAsync(int id, CancellationToken cancellationToken);
        Task<bool> HasRelationAsync(WorkAssignmentRelationType relationType, int sourceId, int targetId, CancellationToken cancellationToken);
        Task<WorkAssignment?> GetWithRelationsAsync(int id, CancellationToken cancellationToken);
        Task<WorkAssignment?> GetFullIncludeAsync(int id, CancellationToken cancellationToken);
        Task<IEnumerable<WorkAssignment>> GetCollectionAsync(int cursor, int take, bool onlyHeadLevelObjects, CancellationToken cancellationToken);
        Task<IEnumerable<WorkAssignment>> GetCollectionFullIncludesAsync(int cursor, int take, bool onlyHeadLevelObjects, CancellationToken cancellationToken);
    }
}
