using MediatR;
using System.ComponentModel.DataAnnotations;
using TaskTracker.Domain.Aggregates.WorkAssignment;

namespace TaskTracker.API.Application.Commands
{
    public class AddWorkAssignmentRelationCommand : IRequest<AddWorkAssignmentRelationCommandResponse>
    {
        [Required]
        public WorkAssignmentRelationType Relation { get; set; }
        [Required]
        public int SourceId { get; set; }
        [Required]
        public int TargetId { get; set; }
    }

    public record AddWorkAssignmentRelationCommandResponse(bool Success) : ApiResponseBase(Success);
}
