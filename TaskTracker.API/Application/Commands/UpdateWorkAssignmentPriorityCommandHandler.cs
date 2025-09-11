using MediatR;
using TaskTracker.Domain.Aggregates.WorkAssignment;

namespace TaskTracker.API.Application.Commands
{
    public class UpdateWorkAssignmentPriorityCommandHandler : IRequestHandler<UpdateWorkAssignmentPriorityCommand, UpdateWorkAssignmentPriorityCommandResponse>
    {
        private readonly ILogger<UpdateWorkAssignmentPriorityCommandHandler> _logger;
        private readonly IWorkAssignmentRepository _workRepository;

        public UpdateWorkAssignmentPriorityCommandHandler(ILogger<UpdateWorkAssignmentPriorityCommandHandler> logger, IWorkAssignmentRepository taskRepository)
        {
            _logger = logger;
            _workRepository = taskRepository;
        }

        public async Task<UpdateWorkAssignmentPriorityCommandResponse> Handle(UpdateWorkAssignmentPriorityCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _workRepository.GetAsync(request.Id, cancellationToken);
                if (entity is not null)
                {
                    entity.SetPriority(request.NewPriority);
                    var r = await _workRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
                    return new UpdateWorkAssignmentPriorityCommandResponse(r > 0);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to update work assignment priority. WorkAssignmentId: '{Id}' and NewPriority: '{Status}'", request.Id, request.NewPriority);
            }
            return new UpdateWorkAssignmentPriorityCommandResponse(false);
        }
    }

}
