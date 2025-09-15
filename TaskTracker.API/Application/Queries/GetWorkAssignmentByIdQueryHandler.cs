using MediatR;
using TaskTracker.API.Application.Dto;
using TaskTracker.API.Application.Extensions;
using TaskTracker.Domain.Aggregates.WorkAssignment;

namespace TaskTracker.API.Application.Queries
{
    public class GetWorkAssignmentByIdQueryHandler : IRequestHandler<GetWorkAssignmentByIdQuery, ApiRequestResult>
    {
        private readonly ILogger<GetWorkAssignmentByIdQueryHandler> _logger;
        private readonly IWorkAssignmentRepository _workRepository;

        public GetWorkAssignmentByIdQueryHandler(ILogger<GetWorkAssignmentByIdQueryHandler> logger, IWorkAssignmentRepository taskRepository)
        {
            _logger = logger;
            _workRepository = taskRepository;
        }

        public async Task<ApiRequestResult> Handle(GetWorkAssignmentByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                WorkAssignment? entity;

                if (request.WithEmbedData)
                {
                    entity = await _workRepository.GetFullIncludeAsync(request.Id, cancellationToken);
                }
                else
                {
                    entity = await _workRepository.GetAsync(request.Id, cancellationToken);
                }

                if (entity is null)
                {
                    return ApiRequestResult.Fail(System.Net.HttpStatusCode.NotFound, "The task does not exists.");
                }
                return ApiRequestResult.Success(entity.ToWorkAssignmentDto());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to get work assignment by id. WorkAssignmentId: '{Id}'", request.Id);
            }

            return ApiRequestResult.Fail(System.Net.HttpStatusCode.InternalServerError);
        }
    }
}
