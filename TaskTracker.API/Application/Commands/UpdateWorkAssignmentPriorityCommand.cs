using MediatR;
using System.ComponentModel.DataAnnotations;
using TaskTracker.Domain.Aggregates.WorkAssignment;

namespace TaskTracker.API.Application.Commands
{
    public class UpdateWorkAssignmentPriorityCommand : IRequest<UpdateWorkAssignmentPriorityCommandResponse>
    {
        /// <summary>
        /// The identifier of the task for which the priority needs to be changed.
        /// </summary>
        [Required]
        public int Id { get; set; }
        /// <summary>
        /// The new priority that needs to be set.
        /// </summary>
        [Required]
        public WorkAssignmentPriority NewPriority { get; set; }
    }

    public record UpdateWorkAssignmentPriorityCommandResponse(bool Success) : ApiResponseBase(Success);
}
