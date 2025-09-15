using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using TaskTracker.Domain.Aggregates.WorkAssignment;

namespace TaskTracker.API.Application.Commands
{
    public class RemoveWorkAssignmentRelationCommand : IRequest<ApiRequestResult>
    {
        /// <summary>
        /// Тип связи между задачами.
        /// </summary>
        [Required]
        [FromRoute(Name = "relationType")]
        public WorkAssignmentRelationType Relation { get; set; }
        /// <summary>
        /// Идентификатор задачи которую нужно отвязать от целевой задачей.
        /// </summary>
        [Required]
        [FromRoute(Name = "sourceId")]
        public int SourceId { get; set; }
        /// <summary>
        /// Идентификатор целевой задачи.
        /// </summary>
        [Required]
        [FromRoute(Name = "targetId")]
        public int TargetId { get; set; }
    }
}
