using MediatR;
using TaskTracker.Domain.Aggregates.WorkAssignment;

namespace TaskTracker.API.Application.Commands
{
    public class ClearHeadWorkAssignmentCommandHandler : IRequestHandler<ClearHeadWorkAssignmentCommand, ClearHeadWorkAssignmentCommandResponse>
    {
        private readonly ILogger<ClearHeadWorkAssignmentCommandHandler> _logger;
        private readonly IWorkAssignmentRepository _taskRepository;

        public ClearHeadWorkAssignmentCommandHandler(ILogger<ClearHeadWorkAssignmentCommandHandler> logger, IWorkAssignmentRepository taskRepository)
        {
            _logger = logger;
            _taskRepository = taskRepository;
        }

        public async Task<ClearHeadWorkAssignmentCommandResponse> Handle(ClearHeadWorkAssignmentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var subWork = await _taskRepository.GetAsync(request.Id, cancellationToken);
                if (subWork != null)
                {
                    subWork.RemoveHeadAssignment();
                    var r = await _taskRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
                    return new ClearHeadWorkAssignmentCommandResponse(r > 0);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to remove head WorkAssignment. WorkAssignmentId: '{ID}'.", request.Id);
            }
            return new ClearHeadWorkAssignmentCommandResponse(false);
        }
    }
}
