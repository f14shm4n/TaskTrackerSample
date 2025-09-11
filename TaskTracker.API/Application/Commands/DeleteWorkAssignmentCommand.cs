using MediatR;
using System.ComponentModel.DataAnnotations;

namespace TaskTracker.API.Application.Commands
{
    public class DeleteWorkAssignmentCommand : IRequest<DeleteWorkAssignmentCommandResponse>
    {
        [Required]
        public int Id { get; set; }

        public bool ReleaseSubTasks { get; set; } = true;
    }

    public record DeleteWorkAssignmentCommandResponse(bool Success) : ApiResponseBase(Success);
}
