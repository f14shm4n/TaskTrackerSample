using MediatR;
using System.ComponentModel.DataAnnotations;
using TaskTracker.Domain.Aggregates.Tasks;

namespace TaskTracker.API.Application.Commands
{
    public class UpdateTaskPriorityCommand : IRequest<UpdateTaskPriorityCommandResponse>
    {
        [Required]
        public int TaskId { get; set; }
        [Required]
        public TaskEntityPriority NewPriority { get; set; }
    }

    public record UpdateTaskPriorityCommandResponse(bool Success) : ApiResponseBase(Success);
}
