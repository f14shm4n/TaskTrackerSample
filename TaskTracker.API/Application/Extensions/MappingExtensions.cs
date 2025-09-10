using Azure.Core;
using System.Threading.Tasks;
using TaskTracker.API.Application.Commands;
using TaskTracker.API.Application.Dto;
using TaskTracker.Domain.Aggregates.Tasks;

namespace TaskTracker.API.Application.Extensions
{
    public static class MappingExtensions
    {
        public static TaskEntity ToTaskEntity(this CreateTaskCommand request)
        {
            return new TaskEntity(request.Title, request.Description, request.Status, request.Priority, request.Author, request.Worker);
        }

        public static TaskDTO ToTaskDto(this TaskEntity task)
        {
            return new TaskDTO
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Status = task.Status,
                Priority = task.Priority,
                Author = task.Author,
                Worker = task.Worker,
            };
        }
    }
}
