using MediatR;
using System.ComponentModel.DataAnnotations;

namespace TaskTracker.API.Application.Commands
{
    public class ClearHeadWorkAssignmentCommand : IRequest<ClearHeadWorkAssignmentCommandResponse>
    {
        /// <summary>
        /// The identifier of the subtask for which the parent task needs to be removed.
        /// </summary>
        [Required]
        public int Id { get; set; }
    }

    public record ClearHeadWorkAssignmentCommandResponse(bool Success) : ApiResponseBase(Success);
}
