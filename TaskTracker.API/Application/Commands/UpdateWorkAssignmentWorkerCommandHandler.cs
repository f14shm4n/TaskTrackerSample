using MediatR;
using TaskTracker.Domain.Aggregates.WorkAssignment;

namespace TaskTracker.API.Application.Commands
{
    public class UpdateWorkAssignmentWorkerCommandHandler : IRequestHandler<UpdateWorkAssignmentWorkerCommand, ApiResponseBase>
    {
        private readonly ILogger<UpdateWorkAssignmentWorkerCommandHandler> _logger;
        private readonly IWorkAssignmentRepository _workRepository;

        public UpdateWorkAssignmentWorkerCommandHandler(ILogger<UpdateWorkAssignmentWorkerCommandHandler> logger, IWorkAssignmentRepository taskRepository)
        {
            _logger = logger;
            _workRepository = taskRepository;
        }

        public async Task<ApiResponseBase> Handle(UpdateWorkAssignmentWorkerCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _workRepository.GetAsync(request.Id, cancellationToken);
                if (entity is null)
                {
                    return new ApiResponseBase("The task does not exits.", System.Net.HttpStatusCode.BadRequest);
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
                return new ApiResponseBase(true);
            }
            catch (Exception ex)
            {
                if (request.Worker is null || request.Worker.Length == 0)
                {
                    _logger.LogError(ex, "Unable to recall work assignment worker. WorkAssignmentId: '{Id}'", request.Id);
                }
                else
                {
                    _logger.LogError(ex, "Unable to update work assignment worker. WorkAssignmentId: '{Id}', NewWorker: '{Worker}'", request.Id, request.Worker);
                }
            }
            return new ApiResponseBase(false, System.Net.HttpStatusCode.InternalServerError);
        }
    }
}
