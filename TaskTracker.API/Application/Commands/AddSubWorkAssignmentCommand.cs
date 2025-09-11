using MediatR;
using System.ComponentModel.DataAnnotations;

namespace TaskTracker.API.Application.Commands
{
    public class AddSubWorkAssignmentCommand : IRequest<AddSubWorkAssignmentCommandResponse>
    {
        [Required]
        public int WorkAssignmentId { get; set; }
        [Required]
        public int SubWorkAssignmentId { get; set; }
    }

    public record AddSubWorkAssignmentCommandResponse(bool Success) : ApiResponseBase(Success);
}
