using TaskTracker.Domain.Abstractions;

namespace TaskTracker.Domain.Aggregates.WorkAssignment
{
    public interface IWorkAssignmentRepository : IRepository<WorkAssignment>
    {
        WorkAssignment Add(WorkAssignment workAssignment);
        Task<WorkAssignment?> GetAsync(int id, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
        Task<bool> ContainsAsync(int id, CancellationToken cancellationToken);
    }
}
