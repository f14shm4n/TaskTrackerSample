using MediatR;
using System.ComponentModel.DataAnnotations;

namespace TaskTracker.API.Application.Commands
{
    public class UpdateWorkAssignmentWorkerCommand : IRequest<UpdateWorkAssignmentWorkerCommandResponse>
    {
        /// <summary>
        /// Идентификатор задачи.
        /// </summary>
        [Required]
        public int Id { get; set; }
        /// <summary>
        /// Новое имя исполнителя.
        /// </summary>
        public string? NewWorker { get; set; }
    }

    public record UpdateWorkAssignmentWorkerCommandResponse(bool Success) : ApiResponseBase(Success);
}
