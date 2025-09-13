using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using TaskTracker.Domain.Aggregates.WorkAssignment;

namespace TaskTracker.API.Application.Commands
{
    public class UpdateWorkAssignmentStatusCommand : IRequest<ApiResponseBase>
    {
        /// <summary>
        /// Идентификатор задачи.
        /// </summary>
        [Required]
        [FromRoute(Name = "id")]
        public int Id { get; set; }
        /// <summary>
        /// Новый статус задачи.
        /// </summary>
        [Required]
        [FromRoute(Name = "status")]
        public WorkAssignmentStatus Status { get; set; }
    }
}
