using MediatR;
using System.ComponentModel.DataAnnotations;
using TaskTracker.Domain.Aggregates.WorkAssignment;

namespace TaskTracker.API.Application.Commands
{
    public class AddWorkAssignmentRelationCommand : IRequest<AddWorkAssignmentRelationCommandResponse>
    {
        /// <summary>
        /// Тип связи между задачами.
        /// </summary>
        [Required]
        public WorkAssignmentRelationType Relation { get; set; }
        /// <summary>
        /// Идентификатор задачи, которая связывается с другой задачей.
        /// </summary>
        [Required]
        public int SourceId { get; set; }
        /// <summary>
        /// Идентификатор задачи, с которой связывается другая задача.
        /// </summary>
        [Required]
        public int TargetId { get; set; }
    }

    public record AddWorkAssignmentRelationCommandResponse(bool Success) : ApiResponseBase(Success);
}
