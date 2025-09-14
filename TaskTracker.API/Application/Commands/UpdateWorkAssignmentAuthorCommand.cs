using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using TaskTracker.Infrastructure;

namespace TaskTracker.API.Application.Commands
{
    public class UpdateWorkAssignmentAuthorCommand : IRequest<ApiResponseBase>
    {
        /// <summary>
        /// Идентификатор задачи.
        /// </summary>
        [Required]
        [FromRoute(Name = "id")]
        public int Id { get; set; }
        /// <summary>
        /// Новое имя автора.
        /// </summary>
        [Required]
        [MaxLength(TaskTrackerDbContext.MaxAuthorLength)]
        [FromRoute(Name = "author")]
        public string Author { get; set; } = null!;
    }
}
