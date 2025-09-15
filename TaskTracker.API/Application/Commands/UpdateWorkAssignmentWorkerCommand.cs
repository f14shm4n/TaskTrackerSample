using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace TaskTracker.API.Application.Commands
{
    public class UpdateWorkAssignmentWorkerCommand : IRequest<ApiRequestResult>
    {
        /// <summary>
        /// Идентификатор задачи.
        /// </summary>
        [Required]
        [FromRoute(Name = "id")]
        public int Id { get; set; }
        /// <summary>
        /// Новое имя исполнителя.
        /// </summary>        
        [FromRoute(Name = "worker")]        
        public string? Worker { get; set; }
    }
}
