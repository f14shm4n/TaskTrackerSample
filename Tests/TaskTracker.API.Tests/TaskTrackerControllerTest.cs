using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Threading.Tasks;
using TaskTracker.API.Application.Commands;
using TaskTracker.Domain.Aggregates.Tasks;

namespace TaskTracker.API.Tests
{
    public class TaskTrackerControllerTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public TaskTrackerControllerTest(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task CreateTask()
        {
            var client = _factory.CreateClient();
            await CreateTaskAsync(client);
        }

        [Fact]
        public async Task DeleteTask()
        {
            var client = _factory.CreateClient();

            var rsp = await client.DeleteAsync($"/TaskTracker/delete-task?taskId={(await CreateTaskAsync(client)).TaskId}");
            var data = await rsp.Content.ReadFromJsonAsync<DeleteTaskCommandResponse>();
            data.Should().NotBeNull();
            data.Success.Should().BeTrue();
        }

        [Fact]
        public async Task UpdateStatus()
        {
            var client = _factory.CreateClient();

            var newTaskData = await CreateTaskAsync(client);
            var rsp = await client.PutAsJsonAsync("/TaskTracker/update-task-status", new UpdateTaskStatusCommand { TaskId = newTaskData.TaskId, NewStatus = TaskEntityStatus.InProgress });
            var data = await rsp.Content.ReadFromJsonAsync<UpdateTaskStatusCommandResponse>();
            data.Should().NotBeNull();
            data.Success.Should().BeTrue();
        }

        [Fact]
        public async Task UpdatePriority()
        {
            var client = _factory.CreateClient();

            var newTaskData = await CreateTaskAsync(client);
            var rsp = await client.PutAsJsonAsync("/TaskTracker/update-task-priority", new UpdateTaskPriorityCommand { TaskId = newTaskData.TaskId, NewPriority = TaskEntityPriority.Medium });
            var data = await rsp.Content.ReadFromJsonAsync<UpdateTaskPriorityCommandResponse>();
            data.Should().NotBeNull();
            data.Success.Should().BeTrue();
        }

        [Fact]
        public async Task UpdateAuthor()
        {
            var client = _factory.CreateClient();

            var newTaskData = await CreateTaskAsync(client);
            var rsp = await client.PutAsJsonAsync("/TaskTracker/update-task-author", new UpdateTaskAuthorCommand { TaskId = newTaskData.TaskId, NewAuthor = "Sergey" });
            var data = await rsp.Content.ReadFromJsonAsync<UpdateTaskAuthorCommandResponse>();
            data.Should().NotBeNull();
            data.Success.Should().BeTrue();
        }


        [Fact]
        public async Task UpdateWorker()
        {
            var client = _factory.CreateClient();

            var newTaskData = await CreateTaskAsync(client);
            var rsp = await client.PutAsJsonAsync("/TaskTracker/update-task-worker", new UpdateTaskWorkerCommand { TaskId = newTaskData.TaskId, NewWorker = "Ivan" });
            var data = await rsp.Content.ReadFromJsonAsync<UpdateTaskWorkerCommandResponse>();
            data.Should().NotBeNull();
            data.Success.Should().BeTrue();
        }

        private static async Task<CreateTaskCommandResponse> CreateTaskAsync(HttpClient client, string title = "Test task #1", string author = "Ivan", TaskEntityStatus status = TaskEntityStatus.New, TaskEntityPriority priority = TaskEntityPriority.Low)
        {
            var command = new CreateTaskCommand
            {
                Title = title,
                Author = author,
                Status = status,
                Priority = priority
            };
            var rsp = await client.PostAsJsonAsync("/TaskTracker/create-task", command);
            var data = await rsp.Content.ReadFromJsonAsync<CreateTaskCommandResponse>();
            data.Should().NotBeNull();
            data.TaskId.Should().BeGreaterThan(0);
            return data;
        }
    }
}