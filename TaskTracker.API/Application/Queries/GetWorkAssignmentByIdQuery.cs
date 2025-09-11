using MediatR;
using TaskTracker.API.Application.Dto;

namespace TaskTracker.API.Application.Queries
{
    public class GetWorkAssignmentByIdQuery : IRequest<GetWorkAssignmentByIdQueryResponse>
    {
        public int Id { get; set; }
    }

    public record GetWorkAssignmentByIdQueryResponse(WorkAssignmentDTO? TaskInfo) : ApiResponseBase(TaskInfo is not null);
}
