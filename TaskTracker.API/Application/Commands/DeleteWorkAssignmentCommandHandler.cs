using MediatR;
using TaskTracker.Domain.Aggregates.WorkAssignment;

namespace TaskTracker.API.Application.Commands
{
    public class DeleteWorkAssignmentCommandHandler : IRequestHandler<DeleteWorkAssignmentCommand, DeleteWorkAssignmentCommandResponse>
    {
        private readonly ILogger<DeleteWorkAssignmentCommandHandler> _logger;
        private readonly IWorkAssignmentRepository _taskRepository;

        public DeleteWorkAssignmentCommandHandler(ILogger<DeleteWorkAssignmentCommandHandler> logger, IWorkAssignmentRepository taskRepository)
        {
            _logger = logger;
            _taskRepository = taskRepository;
        }

        public async Task<DeleteWorkAssignmentCommandResponse> Handle(DeleteWorkAssignmentCommand request, CancellationToken cancellationToken)
        {
            DeleteWorkAssignmentCommandResponse response;
            try
            {
                response = new DeleteWorkAssignmentCommandResponse(await _taskRepository.DeleteAsync(request.Id, cancellationToken));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to delete work assignment. WorkAssignmentId: '{Id}'.", request.Id);
                response = new DeleteWorkAssignmentCommandResponse(false);
            }
            return response;
        }
    }
}
