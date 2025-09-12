using MediatR;
using TaskTracker.API.Application.Dto;
using TaskTracker.API.Application.Extensions;
using TaskTracker.Domain.Aggregates.WorkAssignment;

namespace TaskTracker.API.Application.Queries
{
    public class GetWorkAssignmentByIdQueryHandler : IRequestHandler<GetWorkAssignmentByIdQuery, ApiResponseBase<WorkAssignmentDTO>>
    {
        private readonly ILogger<GetWorkAssignmentByIdQueryHandler> _logger;
        private readonly IWorkAssignmentRepository _workRepository;

        public GetWorkAssignmentByIdQueryHandler(ILogger<GetWorkAssignmentByIdQueryHandler> logger, IWorkAssignmentRepository taskRepository)
        {
            _logger = logger;
            _workRepository = taskRepository;
        }

        public async Task<ApiResponseBase<WorkAssignmentDTO>> Handle(GetWorkAssignmentByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _workRepository.GetWithRelationsAndSubsAsync(request.Id, cancellationToken);
                if (entity is null)
                {
                    return new ApiResponseBase<WorkAssignmentDTO>("The task does not exists.", System.Net.HttpStatusCode.NotFound);
                }
                return new ApiResponseBase<WorkAssignmentDTO>(entity.ToWorkAssignmentDto());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to get work assignment by id. WorkAssignmentId: '{Id}'", request.Id);
            }

            return new ApiResponseBase<WorkAssignmentDTO>(false, System.Net.HttpStatusCode.InternalServerError);
        }
    }
}
