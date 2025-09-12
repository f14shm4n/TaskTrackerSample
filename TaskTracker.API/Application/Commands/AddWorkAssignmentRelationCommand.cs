using MediatR;
using System.ComponentModel.DataAnnotations;
using TaskTracker.Domain.Aggregates.WorkAssignment;

namespace TaskTracker.API.Application.Commands
{
    public class AddWorkAssignmentRelationCommand : IRequest<ApiResponseBase>
    {
        /// <summary>
        /// Тип связи между задачами.
        /// </summary>
        [Required]
        public WorkAssignmentRelationType Relation { get; set; }
        /// <summary>
        /// Идентификатор задачи которую нужно связать с целевой задачей.
        /// </summary>
        [Required]
        public int SourceId { get; set; }
        /// <summary>
        /// Идентификатор целевой задачи.
        /// </summary>
        [Required]
        public int TargetId { get; set; }
    }    
}
