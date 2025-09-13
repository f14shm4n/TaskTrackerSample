using MediatR;
using TaskTracker.Domain.Aggregates.WorkAssignment;

namespace TaskTracker.API.Application.Commands
{
    public class ClearHeadWorkAssignmentCommandHandler : IRequestHandler<ClearHeadWorkAssignmentCommand, ApiResponseBase>
    {
        private readonly ILogger<ClearHeadWorkAssignmentCommandHandler> _logger;
        private readonly IWorkAssignmentRepository _workRepository;

        public ClearHeadWorkAssignmentCommandHandler(ILogger<ClearHeadWorkAssignmentCommandHandler> logger, IWorkAssignmentRepository taskRepository)
        {
            _logger = logger;
            _workRepository = taskRepository;
        }

        public async Task<ApiResponseBase> Handle(ClearHeadWorkAssignmentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var subWork = await _workRepository.GetAsync(request.Id, cancellationToken);
                if (subWork is null)
                {
                    return new ApiResponseBase("The task is not exists.", System.Net.HttpStatusCode.NotFound);
                }

                subWork.RemoveHeadAssignment();
                await _workRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
                return new ApiResponseBase(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to remove head WorkAssignment. WorkAssignmentId: '{ID}'.", request.Id);
            }
            return new ApiResponseBase(false, System.Net.HttpStatusCode.InternalServerError);
        }
    }
}
