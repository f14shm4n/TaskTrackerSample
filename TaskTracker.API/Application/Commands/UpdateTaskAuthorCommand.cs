using MediatR;
using System.ComponentModel.DataAnnotations;

namespace TaskTracker.API.Application.Commands
{
    public class UpdateTaskAuthorCommand : IRequest<UpdateTaskAuthorCommandResponse>
    {
        [Required]
        public int TaskId { get; set; }
        [Required]
        public string NewAuthor { get; set; } = null!;
    }

    public record UpdateTaskAuthorCommandResponse(bool Success) : ApiResponseBase(Success);
}
