using MediatR;
using TaskTracker.Domain.Aggregates.Tasks;

namespace TaskTracker.API.Application.Commands
{
    public class UpdateTaskStatusCommandHandler : IRequestHandler<UpdateTaskStatusCommand, UpdateTaskStatusCommandResponse>
    {
        private readonly ILogger<UpdateTaskStatusCommandHandler> _logger;
        private readonly ITaskRepository _taskRepository;

        public UpdateTaskStatusCommandHandler(ILogger<UpdateTaskStatusCommandHandler> logger, ITaskRepository taskRepository)
        {
            _logger = logger;
            _taskRepository = taskRepository;
        }

        public async Task<UpdateTaskStatusCommandResponse> Handle(UpdateTaskStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _taskRepository.GetAsync(request.TaskId, cancellationToken);
                if (entity is not null)
                {
                    entity.SetStatus(request.NewStatus);
                    var r = await _taskRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
                    return new UpdateTaskStatusCommandResponse(r > 0);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to update task status for TaskId: '{Id}' and NewStatus: '{Status}'", request.TaskId, request.NewStatus);
            }
            return new UpdateTaskStatusCommandResponse(false);
        }
    }

}
