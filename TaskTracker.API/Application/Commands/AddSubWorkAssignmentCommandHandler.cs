using MediatR;
using TaskTracker.Domain.Aggregates.WorkAssignment;

namespace TaskTracker.API.Application.Commands
{
    public class AddSubWorkAssignmentCommandHandler : IRequestHandler<AddSubWorkAssignmentCommand, ApiResponseBase>
    {
        private readonly ILogger<AddSubWorkAssignmentCommandHandler> _logger;
        private readonly IWorkAssignmentRepository _workRepository;

        public AddSubWorkAssignmentCommandHandler(ILogger<AddSubWorkAssignmentCommandHandler> logger, IWorkAssignmentRepository taskRepository)
        {
            _logger = logger;
            _workRepository = taskRepository;
        }

        public async Task<ApiResponseBase> Handle(AddSubWorkAssignmentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // This check can be moved to Validation attribute to the command.
                if (request.WorkAssignmentId == request.SubWorkAssignmentId)
                {
                    return new ApiResponseBase("A task cannot be a subtask of itself.", System.Net.HttpStatusCode.BadRequest);
                }

                if (!await _workRepository.ContainsAsync(request.WorkAssignmentId, cancellationToken))
                {
                    return new ApiResponseBase($"A task with {nameof(request.WorkAssignmentId)}: '{request.WorkAssignmentId}' not exists.", System.Net.HttpStatusCode.NotFound);
                }

                var subWork = await _workRepository.GetAsync(request.SubWorkAssignmentId, cancellationToken);
                if (subWork is null)
                {
                    return new ApiResponseBase($"A task with {nameof(request.SubWorkAssignmentId)}: '{request.SubWorkAssignmentId}' not exists.", System.Net.HttpStatusCode.NotFound);
                }

                if (subWork.HeadAssignemtId == request.WorkAssignmentId)
                {
                    return new ApiResponseBase(true);
                }

                subWork.SetHeadAssignment(request.WorkAssignmentId);
                await _workRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
                return new ApiResponseBase(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to add sub work Assignment. WorkAssignmentId: '{HID}', SubWorkAssignmentId: '{SID}'", request.WorkAssignmentId, request.SubWorkAssignmentId);
            }
            return new ApiResponseBase(false, System.Net.HttpStatusCode.InternalServerError);
        }
    }
}
