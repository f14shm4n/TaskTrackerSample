using MediatR;
using System.ComponentModel.DataAnnotations;
using TaskTracker.Domain.Aggregates.WorkAssignment;

namespace TaskTracker.API.Application.Commands
{
    public class UpdateWorkAssignmentStatusCommand : IRequest<UpdateWorkAssignmentStatusCommandResponse>
    {
        /// <summary>
        /// The identifier of the task for which the status needs to be changed.
        /// </summary>
        [Required]
        public int Id { get; set; }
        /// <summary>
        /// The new task status that needs to be assigned.
        /// </summary>
        [Required]
        public WorkAssignmentStatus NewStatus { get; set; }
    }

    public record UpdateWorkAssignmentStatusCommandResponse(bool Success) : ApiResponseBase(Success);
}
