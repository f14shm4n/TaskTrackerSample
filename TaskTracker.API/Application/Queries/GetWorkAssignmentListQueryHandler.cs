using MediatR;
using TaskTracker.API.Application.Dto;
using TaskTracker.API.Application.Extensions;
using TaskTracker.Domain.Aggregates.WorkAssignment;

namespace TaskTracker.API.Application.Queries
{
    public class GetWorkAssignmentListQueryHandler : IRequestHandler<GetWorkAssignmentListQuery, ApiResponseBase<List<WorkAssignmentDTO>>>
    {
        private readonly ILogger<GetWorkAssignmentListQueryHandler> _logger;
        private readonly IWorkAssignmentRepository _workRepository;

        public GetWorkAssignmentListQueryHandler(ILogger<GetWorkAssignmentListQueryHandler> logger, IWorkAssignmentRepository workRepository)
        {
            _logger = logger;
            _workRepository = workRepository;
        }

        public async Task<ApiResponseBase<List<WorkAssignmentDTO>>> Handle(GetWorkAssignmentListQuery request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.WithEmbedData)
                {
                    return new ApiResponseBase<List<WorkAssignmentDTO>>(
                        (await _workRepository.GetCollectionFullIncludesAsync(request.Cursor, request.Limit, request.OnlyHeadTasks, cancellationToken))
                        .Select(x => x.ToWorkAssignmentDto())
                        .ToList());
                }
                else
                {
                    return new ApiResponseBase<List<WorkAssignmentDTO>>(
                        (await _workRepository.GetCollectionAsync(request.Cursor, request.Limit, request.OnlyHeadTasks, cancellationToken))
                        .Select(x => x.ToWorkAssignmentDtoNoEmbed())
                        .ToList());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to get work assignment list.");
            }
            return new ApiResponseBase<List<WorkAssignmentDTO>>(false, System.Net.HttpStatusCode.InternalServerError);
        }
    }
}
