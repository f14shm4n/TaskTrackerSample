using MediatR;
using TaskTracker.Domain.Aggregates.WorkAssignment;

namespace TaskTracker.API.Application.Commands
{
    public class AddSubWorkAssignmentCommandHandler : IRequestHandler<AddSubWorkAssignmentCommand, AddSubWorkAssignmentCommandResponse>
    {
        private readonly ILogger<AddSubWorkAssignmentCommandHandler> _logger;
        private readonly IWorkAssignmentRepository _taskRepository;

        public AddSubWorkAssignmentCommandHandler(ILogger<AddSubWorkAssignmentCommandHandler> logger, IWorkAssignmentRepository taskRepository)
        {
            _logger = logger;
            _taskRepository = taskRepository;
        }

        public async Task<AddSubWorkAssignmentCommandResponse> Handle(AddSubWorkAssignmentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // This check can be moved to Validation attribute to the command.
                if (request.WorkAssignmentId == request.SubWorkAssignmentId)
                {
                    return Fail();
                }

                if (!await _taskRepository.ContainsAsync(request.WorkAssignmentId, cancellationToken))
                {
                    return Fail();
                }

                var subWork = await _taskRepository.GetAsync(request.SubWorkAssignmentId, cancellationToken);
                if (subWork is null)
                {
                    return Fail();
                }

                subWork.SetHeadAssignment(request.WorkAssignmentId);
                var r = await _taskRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
                return r > 0 ? Success() : Fail();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to add sub work Assignment. WorkAssignmentId: '{HID}', SubWorkAssignmentId: '{SID}'", request.WorkAssignmentId, request.SubWorkAssignmentId);
            }
            return Fail();

            static AddSubWorkAssignmentCommandResponse Fail() => new(false);
            static AddSubWorkAssignmentCommandResponse Success() => new(true);
        }
    }
}
