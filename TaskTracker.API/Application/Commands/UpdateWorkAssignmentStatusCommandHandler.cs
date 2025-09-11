using MediatR;
using TaskTracker.Domain.Aggregates.WorkAssignment;

namespace TaskTracker.API.Application.Commands
{
    public class UpdateWorkAssignmentStatusCommandHandler : IRequestHandler<UpdateWorkAssignmentStatusCommand, UpdateWorkAssignmentStatusCommandResponse>
    {
        private readonly ILogger<UpdateWorkAssignmentStatusCommandHandler> _logger;
        private readonly IWorkAssignmentRepository _workRepository;

        public UpdateWorkAssignmentStatusCommandHandler(ILogger<UpdateWorkAssignmentStatusCommandHandler> logger, IWorkAssignmentRepository taskRepository)
        {
            _logger = logger;
            _workRepository = taskRepository;
        }

        public async Task<UpdateWorkAssignmentStatusCommandResponse> Handle(UpdateWorkAssignmentStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _workRepository.GetAsync(request.Id, cancellationToken);
                if (entity is not null)
                {
                    entity.SetStatus(request.NewStatus);
                    var r = await _workRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
                    return new UpdateWorkAssignmentStatusCommandResponse(r > 0);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to update work assignment status. WorkAssignmentId: '{Id}' and NewStatus: '{Status}'", request.Id, request.NewStatus);
            }
            return new UpdateWorkAssignmentStatusCommandResponse(false);
        }
    }

}
