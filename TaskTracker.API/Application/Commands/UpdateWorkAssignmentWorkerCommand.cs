using MediatR;
using System.ComponentModel.DataAnnotations;

namespace TaskTracker.API.Application.Commands
{
    public class UpdateWorkAssignmentWorkerCommand : IRequest<UpdateWorkAssignmentWorkerCommandResponse>
    {
        [Required]
        public int Id { get; set; }
        public string? NewWorker { get; set; }
    }

    public record UpdateWorkAssignmentWorkerCommandResponse(bool Success) : ApiResponseBase(Success);
}
