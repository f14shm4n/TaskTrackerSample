using MediatR;
using System.ComponentModel.DataAnnotations;
using TaskTracker.Domain.Aggregates.WorkAssignment;

namespace TaskTracker.API.Application.Commands
{
    public class AddWorkAssignmentRelationCommand : IRequest<AddWorkAssignmentRelationCommandResponse>
    {
        /// <summary>
        /// The type of relationship between tasks.
        /// </summary>
        [Required]
        public WorkAssignmentRelationType Relation { get; set; }
        /// <summary>
        /// The identifier of the task that is being linked to another task.
        /// </summary>
        [Required]
        public int SourceId { get; set; }
        /// <summary>
        /// The identifier of the task to which another task is being linked.
        /// </summary>
        [Required]
        public int TargetId { get; set; }
    }

    public record AddWorkAssignmentRelationCommandResponse(bool Success) : ApiResponseBase(Success);
}
