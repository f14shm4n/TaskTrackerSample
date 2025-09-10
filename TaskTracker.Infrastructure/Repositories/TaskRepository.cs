using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTracker.Domain.Abstractions;
using TaskTracker.Domain.Aggregates.Tasks;

namespace TaskTracker.Infrastructure.Repositories
{
    public sealed class TaskRepository : ITaskRepository
    {
        private readonly TaskTrackerDbContext _context;

        public TaskRepository(TaskTrackerDbContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public TaskEntity Add(TaskEntity task)
        {
            return _context.Tasks.Add(task).Entity;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            return (await _context.Tasks.Where(x => x.Id == id).ExecuteDeleteAsync(cancellationToken)) > 0;
        }

        public async Task<TaskEntity?> GetAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Tasks.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<bool> ContainsTaskAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Tasks.AnyAsync(x => x.Id == id, cancellationToken);
        }
    }
}
