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
        /// The title of the task.
        /// </summary>
        public string? Title { get; set; }
        /// <summary>
        /// The status of the task.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public WorkAssignmentStatus Status { get; set; }
        /// <summary>
        /// The priority of the task.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public WorkAssignmentPriority Priority { get; set; }
        /// <summary>
        /// The author of the task.
        /// </summary>
        [Required]
        public string Author { get; set; } = string.Empty;
        /// <summary>
        /// The assignee of the task.
        /// </summary>
        public string? Worker { get; set; }
    }

    public record CreateWorkAssignmentCommandResponse(WorkAssignmentDTO? TaskInfo) : ApiResponseBase(TaskInfo is not null);
}
