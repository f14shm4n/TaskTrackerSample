using MediatR;
using System.ComponentModel.DataAnnotations;
using TaskTracker.Domain.Aggregates.WorkAssignment;

namespace TaskTracker.API.Application.Commands
{
    public class UpdateWorkAssignmentStatusCommand : IRequest<UpdateWorkAssignmentStatusCommandResponse>
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public WorkAssignmentStatus NewStatus { get; set; }
    }

    public record UpdateWorkAssignmentStatusCommandResponse(bool Success) : ApiResponseBase(Success);
}
