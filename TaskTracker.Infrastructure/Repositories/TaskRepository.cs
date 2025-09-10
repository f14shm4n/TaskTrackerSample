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

        public TaskEntity Get(int id)
        {
            throw new NotImplementedException();
        }
    }
}
