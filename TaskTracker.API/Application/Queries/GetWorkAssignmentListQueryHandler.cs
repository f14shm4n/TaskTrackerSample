using MediatR;
using TaskTracker.API.Application.Extensions;
using TaskTracker.Domain.Aggregates.WorkAssignment;

namespace TaskTracker.API.Application.Queries
{
    public class GetWorkAssignmentListQueryHandler : IRequestHandler<GetWorkAssignmentListQuery, ApiRequestResult>
    {
        private readonly ILogger<GetWorkAssignmentListQueryHandler> _logger;
        private readonly IWorkAssignmentRepository _workRepository;

        public GetWorkAssignmentListQueryHandler(ILogger<GetWorkAssignmentListQueryHandler> logger, IWorkAssignmentRepository workRepository)
        {
            _logger = logger;
            _workRepository = workRepository;
        }

        public async Task<ApiRequestResult> Handle(GetWorkAssignmentListQuery request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.WithEmbedData)
                {
                    return ApiRequestResult.Success(
                        (await _workRepository.GetCollectionFullIncludesAsync(request.Cursor, request.Limit, request.OnlyHeadTasks, cancellationToken))
                        .Select(x => x.ToWorkAssignmentDto())
                        .ToList());
                }
                else
                {
                    return ApiRequestResult.Success(
                        (await _workRepository.GetCollectionAsync(request.Cursor, request.Limit, request.OnlyHeadTasks, cancellationToken))
                        .Select(x => x.ToWorkAssignmentDtoNoEmbed())
                        .ToList());
                }
            }
            catch (Exception ex)
            {
                _logger.LogUnableToGetWorkAssignmentList(ex);
            }
            return ApiRequestResult.Fail(System.Net.HttpStatusCode.InternalServerError);
        }
    }
}
