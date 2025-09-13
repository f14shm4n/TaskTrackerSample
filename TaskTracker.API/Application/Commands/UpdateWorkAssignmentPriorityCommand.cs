using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using TaskTracker.Domain.Aggregates.WorkAssignment;

namespace TaskTracker.API.Application.Commands
{
    public class UpdateWorkAssignmentPriorityCommand : IRequest<ApiResponseBase>
    {
        /// <summary>
        /// Идентификатор задачи.
        /// </summary>
        [Required]
        [FromRoute(Name = "id")]
        public int Id { get; set; }
        /// <summary>
        /// Новый приоритет задачи.
        /// </summary>
        [Required]
        [FromRoute(Name = "priority")]
        public WorkAssignmentPriority Priority { get; set; }
    }
}
