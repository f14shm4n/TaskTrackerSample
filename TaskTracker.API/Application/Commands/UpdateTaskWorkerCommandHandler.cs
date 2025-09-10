using MediatR;
using TaskTracker.Domain.Aggregates.Tasks;

namespace TaskTracker.API.Application.Commands
{
    public class UpdateTaskWorkerCommandHandler : IRequestHandler<UpdateTaskWorkerCommand, UpdateTaskWorkerCommandResponse>
    {
        private readonly ILogger<UpdateTaskWorkerCommandHandler> _logger;
        private readonly ITaskRepository _taskRepository;

        public UpdateTaskWorkerCommandHandler(ILogger<UpdateTaskWorkerCommandHandler> logger, ITaskRepository taskRepository)
        {
            _logger = logger;
            _taskRepository = taskRepository;
        }

        public async Task<UpdateTaskWorkerCommandResponse> Handle(UpdateTaskWorkerCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _taskRepository.GetAsync(request.TaskId, cancellationToken);
                if (entity is not null)
                {
                    if (request.NewWorker is null || request.NewWorker.Length == 0)
                    {
                        entity.ClearWorker();
                    }
                    else
                    {
                        entity.SetWorker(request.NewWorker);
                    }
                    var r = await _taskRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
                    return new UpdateTaskWorkerCommandResponse(r > 0);
                }
            }
            catch (Exception ex)
            {
                if (request.NewWorker is null || request.NewWorker.Length == 0)
                {
                    _logger.LogError(ex, "Unable to recall task worker. TaskId: '{Id}'", request.TaskId);
                }
                else
                {
                    _logger.LogError(ex, "Unable to update task worker. TaskId: '{Id}', NewWorker: '{Worker}'", request.TaskId, request.NewWorker);
                }
            }
            return new UpdateTaskWorkerCommandResponse(false);
        }
    }
}
