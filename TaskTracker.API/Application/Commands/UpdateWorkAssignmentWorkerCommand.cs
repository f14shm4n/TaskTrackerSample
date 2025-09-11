using MediatR;
using System.ComponentModel.DataAnnotations;

namespace TaskTracker.API.Application.Commands
{
    public class UpdateWorkAssignmentWorkerCommand : IRequest<UpdateWorkAssignmentWorkerCommandResponse>
    {
        /// <summary>
        /// The identifier of the task for which the assignee needs to be set.
        /// </summary>
        [Required]
        public int Id { get; set; }
        /// <summary>
        /// The name of the task assignee that needs to be set.
        /// </summary>
        public string? NewWorker { get; set; }
    }

    public record UpdateWorkAssignmentWorkerCommandResponse(bool Success) : ApiResponseBase(Success);
}
