using MediatR;
using System.ComponentModel.DataAnnotations;

namespace TaskTracker.API.Application.Commands
{
    public class AddSubWorkAssignmentCommand : IRequest<AddSubWorkAssignmentCommandResponse>
    {
        /// <summary>
        /// The identifier of the task for which a subtask needs to be set.
        /// </summary>
        [Required]
        public int WorkAssignmentId { get; set; }
        /// <summary>
        /// The identifier of the task that needs to be set as a subtask.
        /// </summary>
        [Required]
        public int SubWorkAssignmentId { get; set; }
    }

    public record AddSubWorkAssignmentCommandResponse(bool Success) : ApiResponseBase(Success);
}
