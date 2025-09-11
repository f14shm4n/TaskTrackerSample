using MediatR;
using System.ComponentModel.DataAnnotations;
using TaskTracker.Domain.Aggregates.WorkAssignment;

namespace TaskTracker.API.Application.Commands
{
    public class UpdateWorkAssignmentStatusCommand : IRequest<UpdateWorkAssignmentStatusCommandResponse>
    {
        /// <summary>
        /// Идентификатор задачи, для которой нужно изменить статус.
        /// </summary>
        [Required]
        public int Id { get; set; }
        /// <summary>
        /// Новый статус задачи, который нужно присвоить.
        /// </summary>
        [Required]
        public WorkAssignmentStatus NewStatus { get; set; }
    }

    public record UpdateWorkAssignmentStatusCommandResponse(bool Success) : ApiResponseBase(Success);
}
