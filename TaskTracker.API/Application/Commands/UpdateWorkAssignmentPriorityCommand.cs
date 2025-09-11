using MediatR;
using System.ComponentModel.DataAnnotations;
using TaskTracker.Domain.Aggregates.WorkAssignment;

namespace TaskTracker.API.Application.Commands
{
    public class UpdateWorkAssignmentPriorityCommand : IRequest<UpdateWorkAssignmentPriorityCommandResponse>
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public WorkAssignmentPriority NewPriority { get; set; }
    }

    public record UpdateWorkAssignmentPriorityCommandResponse(bool Success) : ApiResponseBase(Success);
}
