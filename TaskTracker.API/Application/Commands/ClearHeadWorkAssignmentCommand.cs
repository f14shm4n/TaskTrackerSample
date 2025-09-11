using MediatR;
using System.ComponentModel.DataAnnotations;

namespace TaskTracker.API.Application.Commands
{
    public class ClearHeadWorkAssignmentCommand : IRequest<ClearHeadWorkAssignmentCommandResponse>
    {
        /// <summary>
        /// Идентификатор подзадачи.
        /// </summary>
        [Required]
        public int Id { get; set; }
    }

    public record ClearHeadWorkAssignmentCommandResponse(bool Success) : ApiResponseBase(Success);
}
