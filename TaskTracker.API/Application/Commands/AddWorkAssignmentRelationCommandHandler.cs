using MediatR;
using TaskTracker.Domain.Aggregates.WorkAssignment;

namespace TaskTracker.API.Application.Commands
{
    public class AddWorkAssignmentRelationCommandHandler : IRequestHandler<AddWorkAssignmentRelationCommand, ApiResponseBase>
    {
        private readonly ILogger<AddWorkAssignmentRelationCommandHandler> _logger;
        private readonly IWorkAssignmentRepository _workRepository;

        public AddWorkAssignmentRelationCommandHandler(ILogger<AddWorkAssignmentRelationCommandHandler> logger, IWorkAssignmentRepository taskRepository)
        {
            _logger = logger;
            _workRepository = taskRepository;
        }

        public async Task<ApiResponseBase> Handle(AddWorkAssignmentRelationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // This check can be moved to Validation attribute to the command.
                if (request.SourceId == request.TargetId)
                {
                    return new ApiResponseBase("The source and target tasks cannot be the same task.", System.Net.HttpStatusCode.BadRequest);
                }

                if (await _workRepository.HasRelationAsync(request.Relation, request.SourceId, request.TargetId, cancellationToken))
                {
                    return new ApiResponseBase(true);
                }

                var source = await _workRepository.GetAsync(request.SourceId, cancellationToken);
                if (source is null)
                {
                    return new ApiResponseBase("The source task does not exists.", System.Net.HttpStatusCode.NotFound);
                }
                if (!await _workRepository.ContainsAsync(request.TargetId, cancellationToken))
                {
                    return new ApiResponseBase("The target task does not exits.", System.Net.HttpStatusCode.NotFound);
                }

                // Since we have only one type WorkAssignmentRelationType.RelativeTo
                // which means that the relation is two way out
                // Source <--> Target                 
                source.AddOutRelation(request.Relation, request.TargetId);
                source.AddInRelation(request.Relation, request.TargetId);

                await _workRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
                return new ApiResponseBase(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to create relation between work assignment: SourceId: '{SID}', TargetId: '{TID}'", request.SourceId, request.TargetId);
            }
            return new ApiResponseBase(false, System.Net.HttpStatusCode.InternalServerError);
        }
    }
}
