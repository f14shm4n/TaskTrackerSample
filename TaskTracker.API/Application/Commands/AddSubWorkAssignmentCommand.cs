using MediatR;
using System.ComponentModel.DataAnnotations;

namespace TaskTracker.API.Application.Commands
{
    public class AddSubWorkAssignmentCommand : IRequest<AddSubWorkAssignmentCommandResponse>
    {
        /// <summary>
        /// Идентификатор задачи к которой необходимо прикрепить подзадачу.
        /// </summary>
        [Required]
        public int WorkAssignmentId { get; set; }
        /// <summary>
        /// Идентификатор подзадачи.
        /// </summary>
        [Required]
        public int SubWorkAssignmentId { get; set; }
    }

    public record AddSubWorkAssignmentCommandResponse(bool Success) : ApiResponseBase(Success);
}
