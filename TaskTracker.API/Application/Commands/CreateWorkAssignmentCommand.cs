using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TaskTracker.API.Application.Dto;
using TaskTracker.Domain.Aggregates.WorkAssignment;

namespace TaskTracker.API.Application.Commands
{
    public class CreateWorkAssignmentCommand : IRequest<CreateWorkAssignmentCommandResponse>
    {
        public string? Title { get; set; }
        public string? Description { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public WorkAssignmentStatus Status { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public WorkAssignmentPriority Priority { get; set; }

        [Required]
        public string Author { get; set; } = string.Empty;
        public string? Worker { get; set; }
    }

    public record CreateWorkAssignmentCommandResponse(int Id) : ApiResponseBase(Id > 0);
}
