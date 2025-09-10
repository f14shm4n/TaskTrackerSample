using MediatR;
using System.ComponentModel.DataAnnotations;
using TaskTracker.Domain.Aggregates.Tasks;

namespace TaskTracker.API.Application.Commands
{
    public class UpdateTaskStatusCommand : IRequest<UpdateTaskStatusCommandResponse>
    {
        [Required]
        public int TaskId { get; set; }
        [Required]
        public TaskEntityStatus NewStatus { get; set; }
    }

    public record UpdateTaskStatusCommandResponse(bool Success) : ApiResponseBase(Success);
}
