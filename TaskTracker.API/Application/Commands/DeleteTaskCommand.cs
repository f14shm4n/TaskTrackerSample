using MediatR;
using System.ComponentModel.DataAnnotations;

namespace TaskTracker.API.Application.Commands
{
    public class DeleteTaskCommand : IRequest<DeleteTaskCommandResponse>
    {
        [Required]
        public int TaskId { get; set; }
    }

    public record DeleteTaskCommandResponse(bool Success) : ApiResponseBase(Success);
}
