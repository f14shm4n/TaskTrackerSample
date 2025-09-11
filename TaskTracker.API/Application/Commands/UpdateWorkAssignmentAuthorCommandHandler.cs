using MediatR;
using TaskTracker.Domain.Aggregates.WorkAssignment;

namespace TaskTracker.API.Application.Commands
{
    public class UpdateWorkAssignmentAuthorCommandHandler : IRequestHandler<UpdateWorkAssignmentAuthorCommand, UpdateWorkAssignmentAuthorCommandResponse>
    {
        private readonly ILogger<UpdateWorkAssignmentAuthorCommandHandler> _logger;
        private readonly IWorkAssignmentRepository _workRepository;

        public UpdateWorkAssignmentAuthorCommandHandler(ILogger<UpdateWorkAssignmentAuthorCommandHandler> logger, IWorkAssignmentRepository taskRepository)
        {
            _logger = logger;
            _workRepository = taskRepository;
        }

        public async Task<UpdateWorkAssignmentAuthorCommandResponse> Handle(UpdateWorkAssignmentAuthorCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _workRepository.GetAsync(request.Id, cancellationToken);
                if (entity is not null)
                {
                    entity.SetAuthor(request.NewAuthor);
                    var r = await _workRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
                    return new UpdateWorkAssignmentAuthorCommandResponse(r > 0);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to update work assignment author. WorkAssignmentId: '{Id}', NewAuthor: '{Authro}'", request.Id, request.NewAuthor);
            }
            return new UpdateWorkAssignmentAuthorCommandResponse(false);
        }
    }    
}
