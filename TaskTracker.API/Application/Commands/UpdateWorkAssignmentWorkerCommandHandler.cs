using MediatR;
using TaskTracker.API.Application.Extensions;
using TaskTracker.Domain.Aggregates.WorkAssignment;

namespace TaskTracker.API.Application.Commands
{
    public class UpdateWorkAssignmentWorkerCommandHandler : IRequestHandler<UpdateWorkAssignmentWorkerCommand, ApiRequestResult>
    {
        private readonly ILogger<UpdateWorkAssignmentWorkerCommandHandler> _logger;
        private readonly IWorkAssignmentRepository _workRepository;

        public UpdateWorkAssignmentWorkerCommandHandler(ILogger<UpdateWorkAssignmentWorkerCommandHandler> logger, IWorkAssignmentRepository taskRepository)
        {
            _logger = logger;
            _workRepository = taskRepository;
        }

        public async Task<ApiRequestResult> Handle(UpdateWorkAssignmentWorkerCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _workRepository.GetAsync(request.Id, cancellationToken);
                if (entity is null)
                {
                    return ApiRequestResult.NotFound("The task does not exits.");
                }

                if (request.Worker is null || request.Worker.Length == 0)
                {
                    entity.ClearWorker();
                }
                else
                {
                    entity.SetWorker(request.Worker);
                }
                await _workRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
                return ApiRequestResult.Success();
            }
            catch (Exception ex)
            {
                if (request.Worker is null || request.Worker.Length == 0)
                {
                    _logger.LogUnableToRecallWorkAssignmentWorker(request.Id, ex);
                }
                else
                {
                    _logger.LogUnableToUpdateWorkAssignmentWorker(request.Id, request.Worker, ex);
                }
            }
            return ApiRequestResult.InternalServerError();
        }
    }
}
