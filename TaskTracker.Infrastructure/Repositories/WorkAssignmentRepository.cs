using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTracker.Domain.Abstractions;
using TaskTracker.Domain.Aggregates.WorkAssignment;

namespace TaskTracker.Infrastructure.Repositories
{
    public sealed class WorkAssignmentRepository : IWorkAssignmentRepository
    {
        private readonly TaskTrackerDbContext _context;

        public WorkAssignmentRepository(TaskTrackerDbContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public WorkAssignment Add(WorkAssignment workAssignment)
        {
            return _context.WorkAssignments.Add(workAssignment).Entity;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            return (await _context.WorkAssignments.Where(x => x.Id == id).ExecuteDeleteAsync(cancellationToken)) > 0;
        }

        public async Task<WorkAssignment?> GetAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.WorkAssignments.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<bool> ContainsAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.WorkAssignments.AnyAsync(x => x.Id == id, cancellationToken);
        }

        //public Task<bool> HasRelationAsync(int firstTaskId, int secondTaskId, CancellationToken cancellationToken)
        //{
        //    _context.TaskRelationships.Where(x => )
        //}
    }
}
