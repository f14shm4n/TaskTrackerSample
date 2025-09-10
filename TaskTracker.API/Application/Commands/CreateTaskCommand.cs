using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TaskTracker.API.Application.Dto;
using TaskTracker.Domain.Aggregates.Tasks;

namespace TaskTracker.API.Application.Commands
{
    public class CreateTaskCommand : IRequest<CreateTaskCommandResponse>
    {
        public string? Title { get; set; }
        public string? Description { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TaskEntityStatus Status { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TaskEntityPriority Priority { get; set; }

        [Required]
        public string Author { get; set; } = string.Empty;
        public string? Worker { get; set; }
    }

    public record CreateTaskCommandResponse(int TaskId) : ApiResponseBase(TaskId > 0);
}
