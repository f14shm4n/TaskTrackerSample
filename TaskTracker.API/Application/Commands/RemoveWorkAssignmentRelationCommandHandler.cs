using MediatR;
using TaskTracker.Domain.Aggregates.WorkAssignment;

namespace TaskTracker.API.Application.Commands
{
    public class RemoveWorkAssignmentRelationCommandHandler : IRequestHandler<RemoveWorkAssignmentRelationCommand, RemoveWorkAssignmentRelationCommandResponse>
    {
        private readonly ILogger<RemoveWorkAssignmentRelationCommandHandler> _logger;
        private readonly IWorkAssignmentRepository _workRepository;

        public RemoveWorkAssignmentRelationCommandHandler(ILogger<RemoveWorkAssignmentRelationCommandHandler> logger, IWorkAssignmentRepository workAssignmentRepository)
        {
            _logger = logger;
            _workRepository = workAssignmentRepository;
        }

        public async Task<RemoveWorkAssignmentRelationCommandResponse> Handle(RemoveWorkAssignmentRelationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // This check can be moved to Validation attribute to the command.
                if (request.SourceId == request.TargetId)
                {
                    return Fail();
                }

                if (!await _workRepository.HasRelationAsync(request.Relation, request.SourceId, request.TargetId, cancellationToken))
                {
                    return Fail();
                }

                var source = await _workRepository.GetWithRelationsAsync(request.SourceId, cancellationToken);
                if (source is null)
                {
                    return Fail();
                }

                // Since we have only one type WorkAssignmentRelationType.RelativeTo
                // which means that the relation is two way out
                // Source <--> Target
                source.RemoveOutRelation(request.Relation, request.TargetId);
                source.RemoveInRelation(request.Relation, request.TargetId);

                var r = await _workRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
                return r > 0 ? Success() : Fail();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to remove relation between work assignment: SourceId: '{SID}', TargetId: '{TID}'", request.SourceId, request.TargetId);
            }

            return Fail();

            static RemoveWorkAssignmentRelationCommandResponse Fail() => new(false);
            static RemoveWorkAssignmentRelationCommandResponse Success() => new(true);
        }
    }
}
