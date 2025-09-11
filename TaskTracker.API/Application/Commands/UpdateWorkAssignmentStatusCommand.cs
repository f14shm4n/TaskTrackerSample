using MediatR;
using System.ComponentModel.DataAnnotations;
using TaskTracker.Domain.Aggregates.WorkAssignment;

namespace TaskTracker.API.Application.Commands
{
    public class UpdateWorkAssignmentStatusCommand : IRequest<UpdateWorkAssignmentStatusCommandResponse>
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

    public record UpdateWorkAssignmentStatusCommandResponse(bool Success) : ApiResponseBase(Success);
}
