using MediatR;
using TaskTracker.Domain.Aggregates.WorkAssignment;

namespace TaskTracker.API.Application.Commands
{
    public class ClearHeadWorkAssignmentCommandHandler : IRequestHandler<ClearHeadWorkAssignmentCommand, ClearHeadWorkAssignmentCommandResponse>
    {
        private readonly ILogger<ClearHeadWorkAssignmentCommandHandler> _logger;
        private readonly IWorkAssignmentRepository _workRepository;

        public ClearHeadWorkAssignmentCommandHandler(ILogger<ClearHeadWorkAssignmentCommandHandler> logger, IWorkAssignmentRepository taskRepository)
        {
            _logger = logger;
            _workRepository = taskRepository;
        }

        public async Task<ClearHeadWorkAssignmentCommandResponse> Handle(ClearHeadWorkAssignmentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var subWork = await _workRepository.GetAsync(request.Id, cancellationToken);
                if (subWork != null)
                {
                    subWork.RemoveHeadAssignment();
                    var r = await _workRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
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
