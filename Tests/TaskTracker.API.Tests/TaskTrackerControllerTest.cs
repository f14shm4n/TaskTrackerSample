using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Threading.Tasks;
using TaskTracker.API.Application.Commands;
using TaskTracker.Domain.Aggregates.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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
            await DoWorkAsync(async sp =>
            {
                var rsp = await client.PostAsJsonAsync("/TaskTracker/create-task", GetCreateTaskCommand());
                var taskData = await rsp.Content.ReadFromJsonAsync<CreateTaskCommandResponse>();
                taskData.Should().NotBeNull();
                taskData.TaskId.Should().BeGreaterThan(0);

                await sp.GetRequiredService<IMediator>().Send(new DeleteTaskCommand { TaskId = taskData.TaskId });
            });
        }

        [Fact]
        public async Task DeleteTask()
        {
            var client = _factory.CreateClient();

            await DoWorkAsync(async sp =>
            {
                var mediator = sp.GetRequiredService<IMediator>();
                var taskData = await mediator.Send(GetCreateTaskCommand());

                var rsp = await client.DeleteAsync($"/TaskTracker/delete-task?taskId={taskData.TaskId}");
                var data = await rsp.Content.ReadFromJsonAsync<DeleteTaskCommandResponse>();
                data.Should().NotBeNull();
                data.Success.Should().BeTrue();
            });
        }

        [Fact]
        public async Task UpdateStatus()
        {
            var client = _factory.CreateClient();

            await DoWorkAsync(async sp =>
            {
                var mediator = sp.GetRequiredService<IMediator>();
                var taskData = await mediator.Send(GetCreateTaskCommand());

                var rsp = await client.PutAsJsonAsync("/TaskTracker/update-task-status", new UpdateTaskStatusCommand { TaskId = taskData.TaskId, NewStatus = TaskEntityStatus.InProgress });
                var data = await rsp.Content.ReadFromJsonAsync<UpdateTaskStatusCommandResponse>();
                data.Should().NotBeNull();
                data.Success.Should().BeTrue();

                await mediator.Send(new DeleteTaskCommand { TaskId = taskData.TaskId });
            });
        }

        [Fact]
        public async Task UpdatePriority()
        {
            var client = _factory.CreateClient();

            await DoWorkAsync(async sp =>
            {
                var mediator = sp.GetRequiredService<IMediator>();
                var taskData = await mediator.Send(GetCreateTaskCommand());

                var rsp = await client.PutAsJsonAsync("/TaskTracker/update-task-priority", new UpdateTaskPriorityCommand { TaskId = taskData.TaskId, NewPriority = TaskEntityPriority.Medium });
                var data = await rsp.Content.ReadFromJsonAsync<UpdateTaskPriorityCommandResponse>();
                data.Should().NotBeNull();
                data.Success.Should().BeTrue();

                await mediator.Send(new DeleteTaskCommand { TaskId = taskData.TaskId });
            });
        }

        [Fact]
        public async Task UpdateAuthor()
        {
            var client = _factory.CreateClient();

            await DoWorkAsync(async sp =>
            {
                var mediator = sp.GetRequiredService<IMediator>();
                var taskData = await mediator.Send(GetCreateTaskCommand());

                var rsp = await client.PutAsJsonAsync("/TaskTracker/update-task-author", new UpdateTaskAuthorCommand { TaskId = taskData.TaskId, NewAuthor = "Sergey" });
                var data = await rsp.Content.ReadFromJsonAsync<UpdateTaskAuthorCommandResponse>();
                data.Should().NotBeNull();
                data.Success.Should().BeTrue();

                await mediator.Send(new DeleteTaskCommand { TaskId = taskData.TaskId });
            });
        }


        [Fact]
        public async Task UpdateWorker()
        {
            var client = _factory.CreateClient();

            await DoWorkAsync(async sp =>
            {
                var mediator = sp.GetRequiredService<IMediator>();
                var taskData = await mediator.Send(GetCreateTaskCommand());

                var rsp = await client.PutAsJsonAsync("/TaskTracker/update-task-worker", new UpdateTaskWorkerCommand { TaskId = taskData.TaskId, NewWorker = "Ivan" });
                var data = await rsp.Content.ReadFromJsonAsync<UpdateTaskWorkerCommandResponse>();
                data.Should().NotBeNull();
                data.Success.Should().BeTrue();

                await mediator.Send(new DeleteTaskCommand { TaskId = taskData.TaskId });
            });
        }

        [Fact]
        public async Task AddSubTask()
        {
            var client = _factory.CreateClient();

            await DoWorkAsync(async sp =>
            {
                var mediator = sp.GetRequiredService<IMediator>();
                var taskData1 = await mediator.Send(GetCreateTaskCommand(title: "Maste task #1"));
                var taskData2 = await mediator.Send(GetCreateTaskCommand(title: "Sub task #1-1"));

                var rsp = await client.PutAsJsonAsync("/TaskTracker/add-sub-task", new AddSubTaskCommand { TaskId = taskData1.TaskId, SubTaskId = taskData2.TaskId });
                var data = await rsp.Content.ReadFromJsonAsync<AddSubTaskCommandResponse>();
                data.Should().NotBeNull();
                data.Success.Should().BeTrue();

                await mediator.Send(new DeleteTaskCommand { TaskId = taskData2.TaskId });
                await mediator.Send(new DeleteTaskCommand { TaskId = taskData1.TaskId });
            });
        }

        [Fact]
        public async Task ClearMasterTask()
        {
            var client = _factory.CreateClient();

            await DoWorkAsync(async sp =>
            {
                var mediator = sp.GetRequiredService<IMediator>();
                var taskData1 = await mediator.Send(GetCreateTaskCommand(title: "Maste task #1"));
                var taskData2 = await mediator.Send(GetCreateTaskCommand(title: "Sub task #1-1"));
                await mediator.Send(new AddSubTaskCommand { TaskId = taskData1.TaskId, SubTaskId = taskData2.TaskId });

                var rsp = await client.PutAsJsonAsync("/TaskTracker/clear-master-task", new ClearMasterTaskCommand { TaskId = taskData2.TaskId });
                var data = await rsp.Content.ReadFromJsonAsync<ClearMasterTaskCommandResponse>();
                data.Should().NotBeNull();
                data.Success.Should().BeTrue();

                await mediator.Send(new DeleteTaskCommand { TaskId = taskData2.TaskId });
                await mediator.Send(new DeleteTaskCommand { TaskId = taskData1.TaskId });
            });
        }

        private async Task DoWorkAsync(Func<IServiceProvider, Task> work)
        {
            using (var scope = _factory.Services.CreateScope())
            {
                await work(scope.ServiceProvider);
            }
        }

        private static CreateTaskCommand GetCreateTaskCommand(string title = "Test task #1", string author = "Ivan", TaskEntityStatus status = TaskEntityStatus.New, TaskEntityPriority priority = TaskEntityPriority.Low)
        {
            return new CreateTaskCommand
            {
                Title = title,
                Author = author,
                Status = status,
                Priority = priority
            };
        }
    }
}