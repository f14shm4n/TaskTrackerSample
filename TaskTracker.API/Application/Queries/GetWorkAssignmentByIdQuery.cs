using MediatR;
using TaskTracker.API.Application.Dto;

namespace TaskTracker.API.Application.Queries
{
    public class GetWorkAssignmentByIdQuery : IRequest<GetWorkAssignmentByIdQueryResponse>
    {
        /// <summary>
        /// Идентификатор задачи, которую нужно получить.
        /// </summary>
        public int Id { get; set; }
    }

    public record GetWorkAssignmentByIdQueryResponse(WorkAssignmentDTO? TaskInfo) : ApiResponseBase(TaskInfo is not null);
}
