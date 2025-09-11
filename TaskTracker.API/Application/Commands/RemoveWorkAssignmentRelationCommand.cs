using MediatR;
using System.ComponentModel.DataAnnotations;
using TaskTracker.Domain.Aggregates.WorkAssignment;

namespace TaskTracker.API.Application.Commands
{
    public class RemoveWorkAssignmentRelationCommand : IRequest<RemoveWorkAssignmentRelationCommandResponse>
    {
        /// <summary>
        /// The type of relationship between tasks.
        /// </summary>
        [Required]
        public WorkAssignmentRelationType Relation { get; set; }
        /// <summary>
        /// The identifier of the task with the outgoing relationship.
        /// </summary>
        [Required]
        public int SourceId { get; set; }
        /// <summary>
        /// The identifier of the task with the incoming relationship.
        /// </summary>
        [Required]
        public int TargetId { get; set; }
    }

    public record RemoveWorkAssignmentRelationCommandResponse(bool Success) : ApiResponseBase(Success);
}
