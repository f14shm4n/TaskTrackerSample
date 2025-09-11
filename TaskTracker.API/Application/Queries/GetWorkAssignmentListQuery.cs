using MediatR;
using TaskTracker.API.Application.Dto;

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
}
