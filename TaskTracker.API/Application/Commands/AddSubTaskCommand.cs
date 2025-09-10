using MediatR;
using System.ComponentModel.DataAnnotations;

namespace TaskTracker.API.Application.Commands
{
    public class AddSubTaskCommand : IRequest<AddSubTaskCommandResponse>
    {
        [Required]
        public int TaskId { get; set; }
        [Required]
        public int SubTaskId { get; set; }
    }

    public record AddSubTaskCommandResponse(bool Success) : ApiResponseBase(Success);
}
