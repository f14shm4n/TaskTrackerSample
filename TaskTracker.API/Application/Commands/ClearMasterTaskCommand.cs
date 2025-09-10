using MediatR;
using System.ComponentModel.DataAnnotations;

namespace TaskTracker.API.Application.Commands
{
    public class ClearMasterTaskCommand : IRequest<ClearMasterTaskCommandResponse>
    {
        [Required]
        public int TaskId { get; set; }
    }

    public record ClearMasterTaskCommandResponse(bool Success) : ApiResponseBase(Success);
}
