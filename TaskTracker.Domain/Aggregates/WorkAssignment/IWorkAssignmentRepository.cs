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
        Task<WorkAssignment?> GetWithRelationsAndSubsAsync(int id, CancellationToken cancellationToken);
        Task<IEnumerable<WorkAssignment>> GetCollectionAsync(int skip, int take, bool onlyHeadLevelObjects, CancellationToken cancellationToken);
        Task<IEnumerable<WorkAssignment>> GetCollectionWithIncludesAsync(int skip, int take, bool onlyHeadLevelObjects, CancellationToken cancellationToken);
    }
}
