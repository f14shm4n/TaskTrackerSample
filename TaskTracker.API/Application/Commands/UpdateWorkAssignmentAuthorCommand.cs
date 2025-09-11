using MediatR;
using System.ComponentModel.DataAnnotations;

namespace TaskTracker.API.Application.Commands
{
    public class UpdateWorkAssignmentAuthorCommand : IRequest<UpdateWorkAssignmentAuthorCommandResponse>
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string NewAuthor { get; set; } = null!;
    }

    public record UpdateWorkAssignmentAuthorCommandResponse(bool Success) : ApiResponseBase(Success);
}
