using TaskTracker.Domain.Aggregates.WorkAssignment;

namespace TaskTracker.API.Application.Dto
{
    public class WorkAssignmentDTO
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public WorkAssignmentStatus Status { get; set; }
        public WorkAssignmentPriority Priority { get; set; }
        public string Author { get; set; } = string.Empty;
        public string? Worker { get; set; }
    }
}
