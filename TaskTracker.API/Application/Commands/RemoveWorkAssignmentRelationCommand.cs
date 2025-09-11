using MediatR;
using System.ComponentModel.DataAnnotations;
using TaskTracker.Domain.Aggregates.WorkAssignment;

namespace TaskTracker.API.Application.Commands
{
    public class RemoveWorkAssignmentRelationCommand : IRequest<RemoveWorkAssignmentRelationCommandResponse>
    {
        [Required]
        public WorkAssignmentRelationType Relation { get; set; }
        [Required]
        public int SourceId { get; set; }
        [Required]
        public int TargetId { get; set; }
    }

    public record RemoveWorkAssignmentRelationCommandResponse(bool Success) : ApiResponseBase(Success);
}
