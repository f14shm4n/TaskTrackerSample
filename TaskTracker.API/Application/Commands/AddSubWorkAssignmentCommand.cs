using MediatR;
using System.ComponentModel.DataAnnotations;

namespace TaskTracker.API.Application.Commands
{
    public class AddSubWorkAssignmentCommand : IRequest<AddSubWorkAssignmentCommandResponse>
    {
        /// <summary>
        /// Идентификатор задачи, для которой нужно установить подзадачу.
        /// </summary>
        [Required]
        public int WorkAssignmentId { get; set; }
        /// <summary>
        /// Идентификатор задачи, для которую нужно установить как подзадачу.
        /// </summary>
        [Required]
        public int SubWorkAssignmentId { get; set; }
    }

    public record AddSubWorkAssignmentCommandResponse(bool Success) : ApiResponseBase(Success);
}
