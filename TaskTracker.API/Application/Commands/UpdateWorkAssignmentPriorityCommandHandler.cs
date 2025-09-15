using MediatR;
using TaskTracker.Domain.Aggregates.WorkAssignment;

namespace TaskTracker.API.Application.Commands
{
    public class UpdateWorkAssignmentPriorityCommandHandler : IRequestHandler<UpdateWorkAssignmentPriorityCommand, ApiRequestResult>
    {
        private readonly ILogger<UpdateWorkAssignmentPriorityCommandHandler> _logger;
        private readonly IWorkAssignmentRepository _workRepository;

        public UpdateWorkAssignmentPriorityCommandHandler(ILogger<UpdateWorkAssignmentPriorityCommandHandler> logger, IWorkAssignmentRepository taskRepository)
        {
            _logger = logger;
            _workRepository = taskRepository;
        }

        public async Task<ApiRequestResult> Handle(UpdateWorkAssignmentPriorityCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _workRepository.GetAsync(request.Id, cancellationToken);
                if (entity is null)
                {
                    return ApiRequestResult.NotFound("The task does not exits.");
                }

                entity.SetPriority(request.Priority);
                await _workRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
                return ApiRequestResult.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to update work assignment priority. WorkAssignmentId: '{Id}' and NewPriority: '{Status}'", request.Id, request.Priority);
            }
            return ApiRequestResult.InternalServerError();
        }
    }

}
