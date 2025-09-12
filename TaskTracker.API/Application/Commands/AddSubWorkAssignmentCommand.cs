using MediatR;
using System.ComponentModel.DataAnnotations;

namespace TaskTracker.API.Application.Commands
{
    public class AddSubWorkAssignmentCommand : IRequest<ApiResponseBase>
    {
        /// <summary>
        /// Идентификатор задачи к которой необходимо прикрепить подзадачу.
        /// </summary>
        [Required]
        public int WorkAssignmentId { get; set; }
        /// <summary>
        /// Идентификатор подзадачи.
        /// </summary>
        [Required]
        public int SubWorkAssignmentId { get; set; }
    }
}
