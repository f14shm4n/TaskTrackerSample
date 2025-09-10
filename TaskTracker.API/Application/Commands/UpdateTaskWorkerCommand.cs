using MediatR;
using System.ComponentModel.DataAnnotations;

namespace TaskTracker.API.Application.Commands
{
    public class UpdateTaskWorkerCommand : IRequest<UpdateTaskWorkerCommandResponse>
    {
        [Required]
        public int TaskId { get; set; }
        public string? NewWorker { get; set; }
    }

    public record UpdateTaskWorkerCommandResponse(bool Success) : ApiResponseBase(Success);
}
