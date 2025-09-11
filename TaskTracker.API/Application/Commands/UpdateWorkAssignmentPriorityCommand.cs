using MediatR;
using System.ComponentModel.DataAnnotations;
using TaskTracker.Domain.Aggregates.WorkAssignment;

namespace TaskTracker.API.Application.Commands
{
    public class UpdateWorkAssignmentPriorityCommand : IRequest<UpdateWorkAssignmentPriorityCommandResponse>
    {
        /// <summary>
        /// Идентификатор задачи, для которой нужно изменить приоритет.
        /// </summary>
        [Required]
        public int Id { get; set; }
        /// <summary>
        /// Новый приоритет, который нужно задать.
        /// </summary>
        [Required]
        public WorkAssignmentPriority NewPriority { get; set; }
    }

    public record UpdateWorkAssignmentPriorityCommandResponse(bool Success) : ApiResponseBase(Success);
}
