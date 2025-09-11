using MediatR;
using System.ComponentModel.DataAnnotations;

namespace TaskTracker.API.Application.Commands
{
    public class UpdateWorkAssignmentAuthorCommand : IRequest<UpdateWorkAssignmentAuthorCommandResponse>
    {
        /// <summary>
        /// Идентификатор задачи, для которой нужно изменить автора.
        /// </summary>
        [Required]
        public int Id { get; set; }
        /// <summary>
        /// Новой имя автора, которое нужно задать.
        /// </summary>
        [Required]
        public string NewAuthor { get; set; } = null!;
    }

    public record UpdateWorkAssignmentAuthorCommandResponse(bool Success) : ApiResponseBase(Success);
}
