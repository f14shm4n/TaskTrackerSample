using MediatR;
using System.ComponentModel.DataAnnotations;

namespace TaskTracker.API.Application.Commands
{
    public class ClearHeadWorkAssignmentCommand : IRequest<ApiResponseBase>
    {
        /// <summary>
        /// Идентификатор подзадачи.
        /// </summary>
        [Required]
        public int Id { get; set; }
    }
}
