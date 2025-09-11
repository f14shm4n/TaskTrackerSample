using MediatR;
using TaskTracker.API.Application.Dto;
using TaskTracker.API.Application.Extensions;
using TaskTracker.Domain.Aggregates.WorkAssignment;

namespace TaskTracker.API.Application.Queries
{
    public class GetWorkAssignmentListQuery : IRequest<GetWorkAssignmentListQueryResponse>
    {
        /// <summary>
        /// Determines whether the related data of the task should be retrieved.
        /// </summary>
        public bool WithRelatedData { get; set; }
        /// <summary>
        /// Determines whether the only root level tasks should be returned.
        /// </summary>
        public bool OnlyHeadTasks { get; set; }
        /// <summary>
        /// Sets the offset to retrieve the task list. Default: 0;
        /// </summary>
        public int Offset { get; set; } = 0;
        /// <summary>
        /// Sets the size of the task results list. Default: 10.
        /// </summary>
        public int Limit { get; set; } = 10;
    }

    public record GetWorkAssignmentListQueryResponse(List<WorkAssignmentDTO>? TaskList) : ApiResponseBase(TaskList is not null);

    public class GetWorkAssignmentListQueryHandler : IRequestHandler<GetWorkAssignmentListQuery, GetWorkAssignmentListQueryResponse>
    {
        private readonly ILogger<GetWorkAssignmentListQueryHandler> _logger;
        private readonly IWorkAssignmentRepository _workRepository;

        public GetWorkAssignmentListQueryHandler(ILogger<GetWorkAssignmentListQueryHandler> logger, IWorkAssignmentRepository workRepository)
        {
            _logger = logger;
            _workRepository = workRepository;
        }

        public async Task<GetWorkAssignmentListQueryResponse> Handle(GetWorkAssignmentListQuery request, CancellationToken cancellationToken)
        {
            try
            {
                IEnumerable<WorkAssignment> collection;
                if (request.WithRelatedData)
                {
                    collection = await _workRepository.GetCollectionWithIncludesAsync(request.Offset, request.Limit, request.OnlyHeadTasks, cancellationToken);
                }
                else
                {
                    collection = await _workRepository.GetCollectionAsync(request.Offset, request.Limit, request.OnlyHeadTasks, cancellationToken);
                }
                return new GetWorkAssignmentListQueryResponse(collection.Select(x => x.ToWorkAssignmentDto()).ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to get work assignment list.");
            }
            return new GetWorkAssignmentListQueryResponse(null);
        }
    }
}
