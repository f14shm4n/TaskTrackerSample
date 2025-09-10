using MediatR;
using TaskTracker.Domain.Aggregates.Tasks;

namespace TaskTracker.API.Application.Commands
{
    public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, DeleteTaskCommandResponse>
    {
        private readonly ILogger<DeleteTaskCommandHandler> _logger;
        private readonly ITaskRepository _taskRepository;

        public DeleteTaskCommandHandler(ILogger<DeleteTaskCommandHandler> logger, ITaskRepository taskRepository)
        {
            _logger = logger;
            _taskRepository = taskRepository;
        }

        public async Task<DeleteTaskCommandResponse> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {
            DeleteTaskCommandResponse response;
            try
            {
                response = new DeleteTaskCommandResponse(await _taskRepository.DeleteAsync(request.TaskId, cancellationToken));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to delete task. TaskId: '{Id}'.", request.TaskId);
                response = new DeleteTaskCommandResponse(false);
            }
            return response;
        }
    }
}
