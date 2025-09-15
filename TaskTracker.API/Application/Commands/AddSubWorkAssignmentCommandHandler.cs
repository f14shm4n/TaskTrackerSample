using MediatR;
using TaskTracker.Domain.Aggregates.WorkAssignment;

namespace TaskTracker.API.Application.Commands
{
    public class AddSubWorkAssignmentCommandHandler : IRequestHandler<AddSubWorkAssignmentCommand, ApiRequestResult>
    {
        private readonly ILogger<AddSubWorkAssignmentCommandHandler> _logger;
        private readonly IWorkAssignmentRepository _workRepository;

        public AddSubWorkAssignmentCommandHandler(ILogger<AddSubWorkAssignmentCommandHandler> logger, IWorkAssignmentRepository taskRepository)
        {
            _logger = logger;
            _workRepository = taskRepository;
        }

        public async Task<ApiRequestResult> Handle(AddSubWorkAssignmentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // This check can be moved to Validation attribute to the command.
                if (request.WorkAssignmentId == request.SubWorkAssignmentId)
                {
                    return ApiRequestResult.BadRequest("A task cannot be a subtask of itself.");
                }

                if (!await _workRepository.ContainsAsync(request.WorkAssignmentId, cancellationToken))
                {
                    return ApiRequestResult.NotFound($"A task with {nameof(request.WorkAssignmentId)}: '{request.WorkAssignmentId}' not exists.");
                }

                var subWork = await _workRepository.GetAsync(request.SubWorkAssignmentId, cancellationToken);
                if (subWork is null)
                {
                    return ApiRequestResult.NotFound($"A task with {nameof(request.SubWorkAssignmentId)}: '{request.SubWorkAssignmentId}' not exists.");
                }

                if (subWork.HeadAssignmentId == request.WorkAssignmentId)
                {
                    return ApiRequestResult.Success();
                }

                subWork.SetHeadAssignment(request.WorkAssignmentId);
                await _workRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
                return ApiRequestResult.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to add sub work Assignment. WorkAssignmentId: '{HID}', SubWorkAssignmentId: '{SID}'", request.WorkAssignmentId, request.SubWorkAssignmentId);
            }
            return ApiRequestResult.InternalServerError();
        }
    }
}
