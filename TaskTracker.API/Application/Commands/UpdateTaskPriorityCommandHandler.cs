using MediatR;
using TaskTracker.Domain.Aggregates.Tasks;

namespace TaskTracker.API.Application.Commands
{
    public class UpdateTaskPriorityCommandHandler : IRequestHandler<UpdateTaskPriorityCommand, UpdateTaskPriorityCommandResponse>
    {
        private readonly ILogger<UpdateTaskPriorityCommandHandler> _logger;
        private readonly ITaskRepository _taskRepository;

        public UpdateTaskPriorityCommandHandler(ILogger<UpdateTaskPriorityCommandHandler> logger, ITaskRepository taskRepository)
        {
            _logger = logger;
            _taskRepository = taskRepository;
        }

        public async Task<UpdateTaskPriorityCommandResponse> Handle(UpdateTaskPriorityCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _taskRepository.GetAsync(request.TaskId, cancellationToken);
                if (entity is not null)
                {
                    entity.SetPriority(request.NewPriority);
                    var r = await _taskRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
                    return new UpdateTaskPriorityCommandResponse(r > 0);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to update task priority for TaskId: '{Id}' and NewPriority: '{Status}'", request.TaskId, request.NewPriority);
            }
            return new UpdateTaskPriorityCommandResponse(false);
        }
    }

}
