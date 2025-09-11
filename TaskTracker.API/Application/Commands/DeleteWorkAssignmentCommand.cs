using MediatR;
using System.ComponentModel.DataAnnotations;

namespace TaskTracker.API.Application.Commands
{
    public class DeleteWorkAssignmentCommand : IRequest<DeleteWorkAssignmentCommandResponse>
    {
        /// <summary>
        /// Идентификатор задачи, которую нужно удалить.
        /// </summary>
        [Required]
        public int Id { get; set; }
    }

    public record DeleteWorkAssignmentCommandResponse(bool Success) : ApiResponseBase(Success);
}
