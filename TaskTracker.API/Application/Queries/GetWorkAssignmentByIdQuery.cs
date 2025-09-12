using MediatR;
using TaskTracker.API.Application.Dto;

namespace TaskTracker.API.Application.Queries
{
    public class GetWorkAssignmentByIdQuery : IRequest<ApiResponseBase<WorkAssignmentDTO>>
    {
        /// <summary>
        /// Идентификатор задачи, которую нужно получить.
        /// </summary>
        public int Id { get; set; }
    }
}
