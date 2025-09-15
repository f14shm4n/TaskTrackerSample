using MediatR;
using TaskTracker.Domain.Aggregates.WorkAssignment;

namespace TaskTracker.API.Application.Commands
{
    public class UpdateWorkAssignmentAuthorCommandHandler : IRequestHandler<UpdateWorkAssignmentAuthorCommand, ApiRequestResult>
    {
        private readonly ILogger<UpdateWorkAssignmentAuthorCommandHandler> _logger;
        private readonly IWorkAssignmentRepository _workRepository;

        public UpdateWorkAssignmentAuthorCommandHandler(ILogger<UpdateWorkAssignmentAuthorCommandHandler> logger, IWorkAssignmentRepository taskRepository)
        {
            _logger = logger;
            _workRepository = taskRepository;
        }

        public async Task<ApiRequestResult> Handle(UpdateWorkAssignmentAuthorCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _workRepository.GetAsync(request.Id, cancellationToken);
                if (entity is null)
                {
                    return ApiRequestResult.NotFound("The task does not exists.");
                }

                entity.SetAuthor(request.Author);
                await _workRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
                return ApiRequestResult.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to update work assignment author. WorkAssignmentId: '{Id}', NewAuthor: '{Author}'", request.Id, request.Author);
            }
            return ApiRequestResult.InternalServerError();
        }
    }
}
