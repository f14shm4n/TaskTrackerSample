using MediatR;
using TaskTracker.Domain.Aggregates.Tasks;

namespace TaskTracker.API.Application.Commands
{
    public class UpdateTaskAuthorCommandHandler : IRequestHandler<UpdateTaskAuthorCommand, UpdateTaskAuthorCommandResponse>
    {
        private readonly ILogger<UpdateTaskAuthorCommandHandler> _logger;
        private readonly ITaskRepository _taskRepository;

        public UpdateTaskAuthorCommandHandler(ILogger<UpdateTaskAuthorCommandHandler> logger, ITaskRepository taskRepository)
        {
            _logger = logger;
            _taskRepository = taskRepository;
        }

        public async Task<UpdateTaskAuthorCommandResponse> Handle(UpdateTaskAuthorCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _taskRepository.GetAsync(request.TaskId, cancellationToken);
                if (entity is not null)
                {
                    entity.SetAuthor(request.NewAuthor);
                    var r = await _taskRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
                    return new UpdateTaskAuthorCommandResponse(r > 0);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to update task author. TaskId: '{Id}', NewAuthor: '{Authro}'", request.TaskId, request.NewAuthor);
            }
            return new UpdateTaskAuthorCommandResponse(false);
        }
    }    
}
