using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace TaskTracker.API.Application.Commands
{
    public class AddSubWorkAssignmentCommand : IRequest<ApiRequestResult>
    {
        /// <summary>
        /// Идентификатор задачи к которой необходимо прикрепить подзадачу.
        /// </summary>
        [Required]
        [FromRoute(Name = "id")]
        public int WorkAssignmentId { get; set; }
        /// <summary>
        /// Идентификатор подзадачи.
        /// </summary>
        [Required]
        [FromRoute(Name = "subTaskId")]
        public int SubWorkAssignmentId { get; set; }
    }
}
