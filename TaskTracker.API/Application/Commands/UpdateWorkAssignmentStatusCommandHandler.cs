using MediatR;
using TaskTracker.API.Application.Extensions;
using TaskTracker.Domain.Aggregates.WorkAssignment;

namespace TaskTracker.API.Application.Commands
{
    public class UpdateWorkAssignmentStatusCommandHandler : IRequestHandler<UpdateWorkAssignmentStatusCommand, ApiRequestResult>
    {
        private readonly ILogger<UpdateWorkAssignmentStatusCommandHandler> _logger;
        private readonly IWorkAssignmentRepository _workRepository;

        public UpdateWorkAssignmentStatusCommandHandler(ILogger<UpdateWorkAssignmentStatusCommandHandler> logger, IWorkAssignmentRepository taskRepository)
        {
            _logger = logger;
            _workRepository = taskRepository;
        }

        public async Task<ApiRequestResult> Handle(UpdateWorkAssignmentStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _workRepository.GetAsync(request.Id, cancellationToken);
                if (entity is null)
                {
                    return ApiRequestResult.NotFound("The task does not exits.");
                }

                entity.SetStatus(request.Status);
                await _workRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
                return ApiRequestResult.Success();
            }
            catch (Exception ex)
            {
                _logger.LogUnableToUpdateWorkAssignmentStatus(request.Id, request.Status, ex);
            }
            return ApiRequestResult.InternalServerError();
        }
    }

}
