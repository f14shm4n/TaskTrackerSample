using MediatR;
using TaskTracker.Domain.Aggregates.WorkAssignment;

namespace TaskTracker.API.Application.Commands
{
    public class UpdateWorkAssignmentStatusCommandHandler : IRequestHandler<UpdateWorkAssignmentStatusCommand, ApiResponseBase>
    {
        private readonly ILogger<UpdateWorkAssignmentStatusCommandHandler> _logger;
        private readonly IWorkAssignmentRepository _workRepository;

        public UpdateWorkAssignmentStatusCommandHandler(ILogger<UpdateWorkAssignmentStatusCommandHandler> logger, IWorkAssignmentRepository taskRepository)
        {
            _logger = logger;
            _workRepository = taskRepository;
        }

        public async Task<ApiResponseBase> Handle(UpdateWorkAssignmentStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _workRepository.GetAsync(request.Id, cancellationToken);
                if (entity is null)
                {
                    return new ApiResponseBase("The task does not exits.", System.Net.HttpStatusCode.BadRequest);
                }

                entity.SetStatus(request.Status);
                await _workRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
                return new ApiResponseBase(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to update work assignment status. WorkAssignmentId: '{Id}' and NewStatus: '{Status}'", request.Id, request.Status);
            }
            return new ApiResponseBase(false, System.Net.HttpStatusCode.InternalServerError);
        }
    }

}
