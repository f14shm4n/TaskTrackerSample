using MediatR;
using TaskTracker.Domain.Aggregates.WorkAssignment;

namespace TaskTracker.API.Application.Commands
{
    public class DeleteWorkAssignmentCommandHandler : IRequestHandler<DeleteWorkAssignmentCommand, ApiResponseBase>
    {
        private readonly ILogger<DeleteWorkAssignmentCommandHandler> _logger;
        private readonly IWorkAssignmentRepository _workRepository;

        public DeleteWorkAssignmentCommandHandler(ILogger<DeleteWorkAssignmentCommandHandler> logger, IWorkAssignmentRepository taskRepository)
        {
            _logger = logger;
            _workRepository = taskRepository;
        }

        public async Task<ApiResponseBase> Handle(DeleteWorkAssignmentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _workRepository.DeleteAsync(request.Id, cancellationToken);
                return new ApiResponseBase(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to delete work assignment. WorkAssignmentId: '{Id}'.", request.Id);
            }
            return new ApiResponseBase(false, System.Net.HttpStatusCode.InternalServerError);
        }
    }
}
