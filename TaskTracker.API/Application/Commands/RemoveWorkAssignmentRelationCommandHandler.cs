using MediatR;
using TaskTracker.Domain.Aggregates.WorkAssignment;

namespace TaskTracker.API.Application.Commands
{
    public class RemoveWorkAssignmentRelationCommandHandler : IRequestHandler<RemoveWorkAssignmentRelationCommand, ApiRequestResult>
    {
        private readonly ILogger<RemoveWorkAssignmentRelationCommandHandler> _logger;
        private readonly IWorkAssignmentRepository _workRepository;

        public RemoveWorkAssignmentRelationCommandHandler(ILogger<RemoveWorkAssignmentRelationCommandHandler> logger, IWorkAssignmentRepository workAssignmentRepository)
        {
            _logger = logger;
            _workRepository = workAssignmentRepository;
        }

        public async Task<ApiRequestResult> Handle(RemoveWorkAssignmentRelationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // This check can be moved to Validation attribute to the command.
                if (request.SourceId == request.TargetId)
                {
                    return ApiRequestResult.BadRequest("The source task and target task are the same.");
                }

                if (!await _workRepository.HasRelationAsync(request.Relation, request.SourceId, request.TargetId, cancellationToken))
                {
                    return ApiRequestResult.Success();
                }

                var source = (await _workRepository.GetWithRelationsAsync(request.SourceId, cancellationToken))!;
                // Since we have only one type WorkAssignmentRelationType.RelativeTo
                // which means that the relation is two way out
                // Source <--> Target
                source.RemoveOutRelation(request.Relation, request.TargetId);
                source.RemoveInRelation(request.Relation, request.TargetId);

                await _workRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
                return ApiRequestResult.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to remove relation between work assignment: SourceId: '{SID}', TargetId: '{TID}'", request.SourceId, request.TargetId);
            }

            return ApiRequestResult.InternalServerError();
        }
    }
}
