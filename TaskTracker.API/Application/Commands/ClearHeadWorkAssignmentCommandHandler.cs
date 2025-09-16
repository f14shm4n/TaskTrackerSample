using MediatR;
using TaskTracker.API.Application.Extensions;
using TaskTracker.Domain.Aggregates.WorkAssignment;

namespace TaskTracker.API.Application.Commands
{
    public class ClearHeadWorkAssignmentCommandHandler : IRequestHandler<ClearHeadWorkAssignmentCommand, ApiRequestResult>
    {
        private readonly ILogger<ClearHeadWorkAssignmentCommandHandler> _logger;
        private readonly IWorkAssignmentRepository _workRepository;

        public ClearHeadWorkAssignmentCommandHandler(ILogger<ClearHeadWorkAssignmentCommandHandler> logger, IWorkAssignmentRepository taskRepository)
        {
            _logger = logger;
            _workRepository = taskRepository;
        }

        public async Task<ApiRequestResult> Handle(ClearHeadWorkAssignmentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var subWork = await _workRepository.GetAsync(request.Id, cancellationToken);
                if (subWork is null)
                {
                    return ApiRequestResult.NotFound("The task is not exists.");
                }

                subWork.RemoveHeadAssignment();
                await _workRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
                return ApiRequestResult.Success();
            }
            catch (Exception ex)
            {
                _logger.LogUnableToRemoveWorkAssignmentNesting(request.Id, ex);
            }
            return ApiRequestResult.InternalServerError();
        }
    }
}
