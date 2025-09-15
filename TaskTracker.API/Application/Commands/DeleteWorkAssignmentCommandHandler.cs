using MediatR;
using TaskTracker.Domain.Aggregates.WorkAssignment;

namespace TaskTracker.API.Application.Commands
{
    public class DeleteWorkAssignmentCommandHandler : IRequestHandler<DeleteWorkAssignmentCommand, ApiRequestResult>
    {
        private readonly ILogger<DeleteWorkAssignmentCommandHandler> _logger;
        private readonly IWorkAssignmentRepository _workRepository;

        public DeleteWorkAssignmentCommandHandler(ILogger<DeleteWorkAssignmentCommandHandler> logger, IWorkAssignmentRepository taskRepository)
        {
            _logger = logger;
            _workRepository = taskRepository;
        }

        public async Task<ApiRequestResult> Handle(DeleteWorkAssignmentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var r = await _workRepository.DeleteAsync(request.Id, cancellationToken);
                if (!r)
                {
                    return ApiRequestResult.NotFound("The task does not exists.");
                }
                return ApiRequestResult.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to delete work assignment. WorkAssignmentId: '{Id}'.", request.Id);
            }
            return ApiRequestResult.InternalServerError();
        }
    }
}
