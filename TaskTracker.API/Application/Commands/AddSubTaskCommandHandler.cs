using MediatR;
using TaskTracker.Domain.Aggregates.Tasks;

namespace TaskTracker.API.Application.Commands
{
    public class AddSubTaskCommandHandler : IRequestHandler<AddSubTaskCommand, AddSubTaskCommandResponse>
    {
        private readonly ILogger<AddSubTaskCommandHandler> _logger;
        private readonly ITaskRepository _taskRepository;

        public AddSubTaskCommandHandler(ILogger<AddSubTaskCommandHandler> logger, ITaskRepository taskRepository)
        {
            _logger = logger;
            _taskRepository = taskRepository;
        }

        public async Task<AddSubTaskCommandResponse> Handle(AddSubTaskCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // This check can be moved to Validation attribute to the command.
                if (request.TaskId == request.SubTaskId)
                {
                    return Fail();
                }

                if (!await _taskRepository.ContainsTaskAsync(request.TaskId, cancellationToken))
                {
                    return Fail();
                }

                var subTask = await _taskRepository.GetAsync(request.SubTaskId, cancellationToken);
                if (subTask is null)
                {
                    return Fail();
                }

                subTask.SetMasterTask(request.TaskId);
                var r = await _taskRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
                return r > 0 ? Success() : Fail();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to add sub task. TaskId: '{MasterTaskId}', SubTaskId: '{SubTaskId}'", request.TaskId, request.SubTaskId);
            }
            return Fail();

            static AddSubTaskCommandResponse Fail() => new(false);
            static AddSubTaskCommandResponse Success() => new(true);
        }
    }
}
