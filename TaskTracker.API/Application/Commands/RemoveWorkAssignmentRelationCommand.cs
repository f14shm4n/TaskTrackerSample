using MediatR;
using System.ComponentModel.DataAnnotations;
using TaskTracker.Domain.Aggregates.WorkAssignment;

namespace TaskTracker.API.Application.Commands
{
    public class RemoveWorkAssignmentRelationCommand : IRequest<RemoveWorkAssignmentRelationCommandResponse>
    {
        [Required]
        public WorkAssignmentRelationType Relation { get; set; }
        [Required]
        public int SourceId { get; set; }
        [Required]
        public int TargetId { get; set; }
    }

    public record RemoveWorkAssignmentRelationCommandResponse(bool Success) : ApiResponseBase(Success);

    public class RemoveWorkAssignmentRelationCommandHandler : IRequestHandler<RemoveWorkAssignmentRelationCommand, RemoveWorkAssignmentRelationCommandResponse>
    {
        private readonly ILogger<RemoveWorkAssignmentRelationCommandHandler> _logger;
        private readonly IWorkAssignmentRepository _workRepository;

        public RemoveWorkAssignmentRelationCommandHandler(ILogger<RemoveWorkAssignmentRelationCommandHandler> logger, IWorkAssignmentRepository workAssignmentRepository)
        {
            _logger = logger;
            _workRepository = workAssignmentRepository;
        }

        public async Task<RemoveWorkAssignmentRelationCommandResponse> Handle(RemoveWorkAssignmentRelationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // This check can be moved to Validation attribute to the command.
                if (request.SourceId == request.TargetId)
                {
                    return Fail();
                }

                if (!await _workRepository.HasRelationAsync(request.Relation, request.SourceId, request.TargetId, cancellationToken))
                {
                    return Fail();
                }

                var source = await _workRepository.GetWithOutRelationsAsync(request.SourceId, cancellationToken);
                var target = await _workRepository.GetWithOutRelationsAsync(request.TargetId, cancellationToken);

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
                source.RemoveOutRelation(request.Relation, target.Id);
                target.RemoveOutRelation(request.Relation, source.Id);

                var r = await _workRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
                return r > 0 ? Success() : Fail();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to remove relation between work assignment: SourceId: '{SID}', TargetId: '{TID}'", request.SourceId, request.TargetId);
            }

            return Fail();

            static RemoveWorkAssignmentRelationCommandResponse Fail() => new(false);
            static RemoveWorkAssignmentRelationCommandResponse Success() => new(true);
        }
    }
}
