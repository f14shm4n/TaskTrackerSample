using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TaskTracker.API.Application.Dto;
using TaskTracker.Domain.Aggregates.WorkAssignment;

namespace TaskTracker.API.Application.Commands
{
    public class CreateWorkAssignmentCommand : IRequest<CreateWorkAssignmentCommandResponse>
    {
        /// <summary>
        /// Заголовок задачи.
        /// </summary>
        public string? Title { get; set; }
        /// <summary>
        /// Статус задачи.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public WorkAssignmentStatus Status { get; set; }
        /// <summary>
        /// Приоритет задачи.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public WorkAssignmentPriority Priority { get; set; }
        /// <summary>
        /// Имя автора.
        /// </summary>
        [Required]
        public string Author { get; set; } = string.Empty;
        /// <summary>
        /// Имя исполнителя.
        /// </summary>
        public string? Worker { get; set; }
    }

    public record CreateWorkAssignmentCommandResponse(WorkAssignmentDTO? TaskInfo) : ApiResponseBase(TaskInfo is not null);
}
