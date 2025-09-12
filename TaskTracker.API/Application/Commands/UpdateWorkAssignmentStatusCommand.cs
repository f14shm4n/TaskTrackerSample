using MediatR;
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
        public int Id { get; set; }
        /// <summary>
        /// Новый статус задачи.
        /// </summary>
        [Required]
        public WorkAssignmentStatus NewStatus { get; set; }
    }
}
