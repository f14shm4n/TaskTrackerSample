using Azure.Core;
using System.Threading.Tasks;
using TaskTracker.API.Application.Commands;
using TaskTracker.API.Application.Dto;
using TaskTracker.Domain.Aggregates.WorkAssignment;

namespace TaskTracker.API.Application.Extensions
{
    public static class MappingExtensions
    {
        public static WorkAssignment ToWorkAssignment(this CreateWorkAssignmentCommand request)
        {
            return new WorkAssignment(request.Title, request.Status, request.Priority, request.Author, request.Worker);
        }

        public static WorkAssignmentDTO ToWorkAssignmentDto(this WorkAssignment workAssignment)
        {
            return new WorkAssignmentDTO
            {
                Id = workAssignment.Id,
                Title = workAssignment.Title,
                Status = workAssignment.Status,
                Priority = workAssignment.Priority,
                Author = workAssignment.Author,
                Worker = workAssignment.Worker,
            };
        }
    }
}
