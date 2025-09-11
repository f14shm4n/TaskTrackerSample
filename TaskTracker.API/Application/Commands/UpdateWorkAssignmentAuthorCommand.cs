using MediatR;
using System.ComponentModel.DataAnnotations;

namespace TaskTracker.API.Application.Commands
{
    public class UpdateWorkAssignmentAuthorCommand : IRequest<UpdateWorkAssignmentAuthorCommandResponse>
    {
        /// <summary>
        /// The identifier of the task for which the author needs to be changed.
        /// </summary>
        [Required]
        public int Id { get; set; }
        /// <summary>
        /// The new author name that needs to be set.
        /// </summary>
        [Required]
        public string NewAuthor { get; set; } = null!;
    }

    public record UpdateWorkAssignmentAuthorCommandResponse(bool Success) : ApiResponseBase(Success);
}
