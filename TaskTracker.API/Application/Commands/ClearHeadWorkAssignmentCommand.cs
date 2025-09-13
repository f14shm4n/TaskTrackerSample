using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace TaskTracker.API.Application.Commands
{
    public class ClearHeadWorkAssignmentCommand : IRequest<ApiResponseBase>
    {
        /// <summary>
        /// Идентификатор подзадачи.
        /// </summary>
        [Required]
        [FromRoute(Name = "id")]
        public int Id { get; set; }
    }
}
