using MediatR;
using System.ComponentModel.DataAnnotations;

namespace TaskTracker.API.Application.Commands
{
    public class UpdateWorkAssignmentAuthorCommand : IRequest<UpdateWorkAssignmentAuthorCommandResponse>
    {
        /// <summary>
        /// Идентификатор задачи.
        /// </summary>
        [Required]
        public int Id { get; set; }
        /// <summary>
        /// Новое имя автора.
        /// </summary>
        [Required]
        public string NewAuthor { get; set; } = null!;
    }

    public record UpdateWorkAssignmentAuthorCommandResponse(bool Success) : ApiResponseBase(Success);
}
