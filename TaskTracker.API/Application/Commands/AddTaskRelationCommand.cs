using MediatR;
using System.ComponentModel.DataAnnotations;
using TaskTracker.Domain.Aggregates.WorkAssignment;

namespace TaskTracker.API.Application.Commands
{
    public class AddTaskRelationCommand : IRequest<AddTaskRelationCommandResponse>
    {
        [Required]
        public int SourceTaskId { get; set; }
        [Required]
        public int TargetTaskId { get; set; }
    }

    public record AddTaskRelationCommandResponse(bool Success) : ApiResponseBase(Success);

    public class AddTaskRelationCommandHandler : IRequestHandler<AddTaskRelationCommand, AddTaskRelationCommandResponse>
    {
        private readonly ILogger<AddTaskRelationCommandHandler> _logger;
        private readonly IWorkAssignmentRepository _taskRepository;

        public AddTaskRelationCommandHandler(ILogger<AddTaskRelationCommandHandler> logger, IWorkAssignmentRepository taskRepository)
        {
            _logger = logger;
            _taskRepository = taskRepository;
        }

        public async Task<AddTaskRelationCommandResponse> Handle(AddTaskRelationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // This check can be moved to Validation attribute to the command.
                if (request.SourceTaskId == request.TargetTaskId)
                {
                    return Fail();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to create relation between tasks: SourceTaskId: '{SID}', TargetTaskId: '{TID}'", request.SourceTaskId, request.TargetTaskId);
            }
            return Fail();

            static AddTaskRelationCommandResponse Fail() => new(false);
            static AddTaskRelationCommandResponse Success() => new(true);
        }
    }
}
