using MediatR;
using TaskTracker.Domain.Aggregates.WorkAssignment;

namespace TaskTracker.API.Application.Commands
{
    public class AddWorkAssignmentRelationCommandHandler : IRequestHandler<AddWorkAssignmentRelationCommand, AddWorkAssignmentRelationCommandResponse>
    {
        private readonly ILogger<AddWorkAssignmentRelationCommandHandler> _logger;
        private readonly IWorkAssignmentRepository _workRepository;

        public AddWorkAssignmentRelationCommandHandler(ILogger<AddWorkAssignmentRelationCommandHandler> logger, IWorkAssignmentRepository taskRepository)
        {
            _logger = logger;
            _workRepository = taskRepository;
        }

        public async Task<AddWorkAssignmentRelationCommandResponse> Handle(AddWorkAssignmentRelationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // This check can be moved to Validation attribute to the command.
                if (request.SourceId == request.TargetId)
                {
                    return Fail();
                }

                if (await _workRepository.HasRelationAsync(request.Relation, request.SourceId, request.TargetId, cancellationToken))
                {
                    return Fail();
                }

                var source = await _workRepository.GetAsync(request.SourceId, cancellationToken);
                var target = await _workRepository.GetAsync(request.TargetId, cancellationToken);

                if (source is null)
                {
                    return Fail();
                }
                if (target is null)
                {
                    return Fail();
                }

                // Since we have only one type WorkAssignmentRelationType.RelativeTo
                // which means that the realtion is two way out
                // Source <--> Target
                source.AddOutRelation(request.Relation, target.Id);
                target.AddOutRelation(request.Relation, source.Id);

                var r = await _workRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
                return r > 0 ? Success() : Fail();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to create relation between work assignment: SourceId: '{SID}', TargetId: '{TID}'", request.SourceId, request.TargetId);
            }
            return Fail();

            static AddWorkAssignmentRelationCommandResponse Fail() => new(false);
            static AddWorkAssignmentRelationCommandResponse Success() => new(true);
        }
    }
}
