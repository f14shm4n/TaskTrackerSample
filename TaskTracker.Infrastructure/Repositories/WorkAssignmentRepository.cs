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

        public Task<WorkAssignment?> GetAsync(int id, CancellationToken cancellationToken)
        {
            return _context.WorkAssignments.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public Task<WorkAssignment?> GetWithOutRelationsAsync(int id, CancellationToken cancellationToken)
        {
            return _context.WorkAssignments.Include(x => x.OutRelations).SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public Task<bool> ContainsAsync(int id, CancellationToken cancellationToken)
        {
            return _context.WorkAssignments.AnyAsync(x => x.Id == id, cancellationToken);
        }

        public Task<bool> HasRelationAsync(WorkAssignmentRelationType relationType, int sourceId, int targetId, CancellationToken cancellationToken)
        {
            return _context.WorkAssignmentRelationships.AnyAsync(x => x.Relation == relationType && x.SourceWorkAssignmentId == sourceId && x.TargetWorkAssignmentId == targetId, cancellationToken);
        }
    }
}
