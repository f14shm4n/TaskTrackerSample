using TaskTracker.Domain.Abstractions;

namespace TaskTracker.Domain.Aggregates.Tasks
{
    public interface ITaskRepository : IRepository<TaskEntity>
    {
        TaskEntity Add(TaskEntity task);
        Task<TaskEntity?> GetAsync(int id, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
        Task<bool> ContainsTaskAsync(int id, CancellationToken cancellationToken);
    }
}
