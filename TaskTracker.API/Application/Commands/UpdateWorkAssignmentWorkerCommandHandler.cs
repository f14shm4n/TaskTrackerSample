using MediatR;
using TaskTracker.Domain.Aggregates.WorkAssignment;

namespace TaskTracker.API.Application.Commands
{
    public class UpdateWorkAssignmentWorkerCommandHandler : IRequestHandler<UpdateWorkAssignmentWorkerCommand, UpdateWorkAssignmentWorkerCommandResponse>
    {
        private readonly ILogger<UpdateWorkAssignmentWorkerCommandHandler> _logger;
        private readonly IWorkAssignmentRepository _taskRepository;

        public UpdateWorkAssignmentWorkerCommandHandler(ILogger<UpdateWorkAssignmentWorkerCommandHandler> logger, IWorkAssignmentRepository taskRepository)
        {
            _logger = logger;
            _taskRepository = taskRepository;
        }

        public async Task<UpdateWorkAssignmentWorkerCommandResponse> Handle(UpdateWorkAssignmentWorkerCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _taskRepository.GetAsync(request.Id, cancellationToken);
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
                    return new UpdateWorkAssignmentWorkerCommandResponse(r > 0);
                }
            }
            catch (Exception ex)
            {
                if (request.NewWorker is null || request.NewWorker.Length == 0)
                {
                    _logger.LogError(ex, "Unable to recall work assignment worker. WorkAssignmentId: '{Id}'", request.Id);
                }
                else
                {
                    _logger.LogError(ex, "Unable to update work assignment worker. WorkAssignmentId: '{Id}', NewWorker: '{Worker}'", request.Id, request.NewWorker);
                }
            }
            return new UpdateWorkAssignmentWorkerCommandResponse(false);
        }
    }
}
