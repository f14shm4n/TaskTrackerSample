using MediatR;
using System.ComponentModel.DataAnnotations;

namespace TaskTracker.API.Application.Commands
{
    public class UpdateWorkAssignmentWorkerCommand : IRequest<UpdateWorkAssignmentWorkerCommandResponse>
    {
        /// <summary>
        /// Идентификатор задачи, для которой нужно установить исполнителя.
        /// </summary>
        [Required]
        public int Id { get; set; }
        /// <summary>
        /// Имя исполнителя задачи, которое нужно задать.
        /// </summary>
        public string? NewWorker { get; set; }
    }

    public record UpdateWorkAssignmentWorkerCommandResponse(bool Success) : ApiResponseBase(Success);
}
