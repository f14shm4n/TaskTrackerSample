using MediatR;
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
        public int Id { get; set; }
        /// <summary>
        /// Новый приоритет задачи.
        /// </summary>
        [Required]
        public WorkAssignmentPriority NewPriority { get; set; }
    }
}
