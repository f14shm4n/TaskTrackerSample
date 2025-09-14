using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<WorkAssignmentRepository> _logger;
        private readonly TaskTrackerDbContext _context;

        public WorkAssignmentRepository(TaskTrackerDbContext context, ILogger<WorkAssignmentRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IUnitOfWork UnitOfWork => _context;

        public WorkAssignment Add(WorkAssignment workAssignment)
        {
            return _context.WorkAssignments.Add(workAssignment).Entity;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            int rCount = 0;
            await using (var transaction = await _context.Database.BeginTransactionAsync(cancellationToken))
            {
                try
                {
                    await _context.WorkAssignmentRelationships
                        .Where(x => x.SourceWorkAssignmentId == id || x.TargetWorkAssignmentId == id)
                        .ExecuteDeleteAsync(cancellationToken);

                    await _context.WorkAssignments
                        .Where(x => x.HeadAssignemtId == id)
                        .ExecuteUpdateAsync(setters => setters.SetProperty(x => x.HeadAssignemtId, (int?)null), cancellationToken);

                    rCount = await _context.WorkAssignments.Where(x => x.Id == id).ExecuteDeleteAsync(cancellationToken);

                    await transaction.CommitAsync(cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unable to commit transaction while deleting work assignment. WorkAssignmentId: '{ID}'", id);
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
            }
            return rCount > 0;
        }

        public Task<WorkAssignment?> GetAsync(int id, CancellationToken cancellationToken)
        {
            return _context.WorkAssignments.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public Task<WorkAssignment?> GetWithRelationsAsync(int id, CancellationToken cancellationToken)
        {
            return WithRelationsIncludes(_context.WorkAssignments)
                .AsSplitQuery()
                .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public Task<WorkAssignment?> GetFullIncludeAsync(int id, CancellationToken cancellationToken)
        {
            return WithFullIncludes(_context.WorkAssignments)
                .AsSplitQuery()
                .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<IEnumerable<WorkAssignment>> GetCollectionAsync(int cursor, int take, bool onlyHeadLevelObjects, CancellationToken cancellationToken)
        {
            if (cursor > 0)
            {
                return await GetInitQueryCollection(onlyHeadLevelObjects)
                     .Where(x => x.Id > cursor)
                     .Take(take)
                     .ToListAsync(cancellationToken);
            }
            return await GetInitQueryCollection(onlyHeadLevelObjects).Take(take).ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<WorkAssignment>> GetCollectionFullIncludesAsync(int cursor, int take, bool onlyHeadLevelObjects, CancellationToken cancellationToken)
        {
            if (cursor > 0)
            {
                return await WithFullIncludes(GetInitQueryCollection(onlyHeadLevelObjects).Where(x => x.Id > cursor))
                    .AsSplitQuery()
                    .Take(take)
                    .ToListAsync(cancellationToken);
            }
            return await WithFullIncludes(GetInitQueryCollection(onlyHeadLevelObjects))
                    .AsSplitQuery()
                    .Take(take)
                    .ToListAsync(cancellationToken);
        }

        public Task<bool> ContainsAsync(int id, CancellationToken cancellationToken)
        {
            return _context.WorkAssignments.AnyAsync(x => x.Id == id, cancellationToken);
        }

        public Task<bool> HasRelationAsync(WorkAssignmentRelationType relationType, int sourceId, int targetId, CancellationToken cancellationToken)
        {
            return _context.WorkAssignmentRelationships.AnyAsync(x => x.Relation == relationType && x.SourceWorkAssignmentId == sourceId && x.TargetWorkAssignmentId == targetId, cancellationToken);
        }

        private IQueryable<WorkAssignment> GetInitQueryCollection(bool onlyHeadLevelObjects)
        {
            return onlyHeadLevelObjects ? _context.WorkAssignments.Where(x => x.HeadAssignemtId == null) : _context.WorkAssignments;
        }

        private static IQueryable<WorkAssignment> WithRelationsIncludes(IQueryable<WorkAssignment> query)
        {
            return query
                .Include(x => x.OutRelations)
                .Include(x => x.InRelations);
        }

        private static IQueryable<WorkAssignment> WithFullIncludes(IQueryable<WorkAssignment> query)
        {
            return query
                .Include(x => x.SubAssignment)
                .Include(x => x.OutRelations)
                .Include(x => x.InRelations)
                .Include(x => x.HeadAssignment);
        }
    }
}
