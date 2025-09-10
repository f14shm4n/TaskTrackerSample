using MediatR;
using System.ComponentModel.DataAnnotations;
using TaskTracker.Domain.Aggregates.Tasks;

namespace TaskTracker.API.Application.Commands
{
    public class ClearMasterTaskCommand : IRequest<ClearMasterTaskCommandResponse>
    {
        [Required]
        public int TaskId { get; set; }
    }

    public record ClearMasterTaskCommandResponse(bool Success) : ApiResponseBase(Success);

    public class ClearMasterTaskCommandHandler : IRequestHandler<ClearMasterTaskCommand, ClearMasterTaskCommandResponse>
    {
        private readonly ILogger<ClearMasterTaskCommandHandler> _logger;
        private readonly ITaskRepository _taskRepository;

        public ClearMasterTaskCommandHandler(ILogger<ClearMasterTaskCommandHandler> logger, ITaskRepository taskRepository)
        {
            _logger = logger;
            _taskRepository = taskRepository;
        }

        public async Task<ClearMasterTaskCommandResponse> Handle(ClearMasterTaskCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var subTask = await _taskRepository.GetAsync(request.TaskId, cancellationToken);
                if (subTask != null)
                {
                    subTask.RemoveMasterTask();
                    var r = await _taskRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
                    return new ClearMasterTaskCommandResponse(r > 0);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to remove as sub task. TaskId: '{TaskId}'.", request.TaskId);
            }
            return new ClearMasterTaskCommandResponse(false);
        }
    }
}
