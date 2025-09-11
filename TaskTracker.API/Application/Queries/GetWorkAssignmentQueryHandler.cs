using MediatR;
using TaskTracker.API.Application.Dto;
using TaskTracker.API.Application.Extensions;
using TaskTracker.Domain.Aggregates.WorkAssignment;

namespace TaskTracker.API.Application.Queries
{
    public class GetWorkAssignmentQueryHandler : IRequestHandler<GetWorkAssignmentByIdQuery, GetWorkAssignmentByIdQueryResponse>
    {
        private readonly ILogger<GetWorkAssignmentQueryHandler> _logger;
        private readonly IWorkAssignmentRepository _workRepository;

        public GetWorkAssignmentQueryHandler(ILogger<GetWorkAssignmentQueryHandler> logger, IWorkAssignmentRepository taskRepository)
        {
            _logger = logger;
            _workRepository = taskRepository;
        }

        public async Task<GetWorkAssignmentByIdQueryResponse> Handle(GetWorkAssignmentByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _workRepository.GetWithRelationsAndSubsAsync(request.Id, cancellationToken);
                if (entity is null)
                {
                    return new GetWorkAssignmentByIdQueryResponse(null);
                }
                return new GetWorkAssignmentByIdQueryResponse(entity.ToWorkAssignmentDto());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to get work assignment by id. WorkAssignmentId: '{Id}'", request.Id);
            }

            return new GetWorkAssignmentByIdQueryResponse(null);
        }
    }
}
