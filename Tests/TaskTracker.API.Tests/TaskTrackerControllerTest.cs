using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using TaskTracker.API.Application.Commands;
using TaskTracker.API.Application.Queries;
using TaskTracker.Domain.Aggregates.WorkAssignment;

namespace TaskTracker.API.Tests
{
    public class TaskTrackerControllerTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly JsonSerializerOptions _jsonOptions = new(JsonSerializerDefaults.Web)
        {
            Converters = { new JsonStringEnumConverter() }
        };

        public TaskTrackerControllerTest(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task CreateWorkAssignment()
        {
            var client = await CreateClientWithJwt();

            await DoWorkAsync(async sp =>
            {
                var rsp = await client.PostAsJsonAsync("/TaskTracker/create-task", GetCreateTaskCommand());
                var taskData = await rsp.Content.ReadFromJsonAsync<CreateWorkAssignmentCommandResponse>();
                taskData.Should().NotBeNull();
                taskData.Id.Should().BeGreaterThan(0);

                await sp.GetRequiredService<IMediator>().Send(new DeleteWorkAssignmentCommand { Id = taskData.Id });
            });
        }

        [Fact]
        public async Task DeleteWorkAssignment()
        {
            var client = await CreateClientWithJwt();

            await DoWorkAsync(async sp =>
            {
                var mediator = sp.GetRequiredService<IMediator>();
                var taskData = await mediator.Send(GetCreateTaskCommand());

                var rsp = await client.DeleteAsync($"/TaskTracker/delete-task?id={taskData.Id}");
                var data = await rsp.Content.ReadFromJsonAsync<DeleteWorkAssignmentCommandResponse>();
                data.Should().NotBeNull();
                data.Success.Should().BeTrue();
            });
        }

        [Fact]
        public async Task UpdateWorkAssignmentStatus()
        {
            var client = await CreateClientWithJwt();

            await DoWorkAsync(async sp =>
            {
                var mediator = sp.GetRequiredService<IMediator>();
                var taskData = await mediator.Send(GetCreateTaskCommand());

                var rsp = await client.PutAsJsonAsync("/TaskTracker/update-task-status", new UpdateWorkAssignmentStatusCommand { Id = taskData.Id, NewStatus = WorkAssignmentStatus.InProgress });
                var data = await rsp.Content.ReadFromJsonAsync<UpdateWorkAssignmentStatusCommandResponse>();
                data.Should().NotBeNull();
                data.Success.Should().BeTrue();

                await mediator.Send(new DeleteWorkAssignmentCommand { Id = taskData.Id });
            });
        }

        [Fact]
        public async Task UpdateWorkAssignmentPriority()
        {
            var client = await CreateClientWithJwt();

            await DoWorkAsync(async sp =>
            {
                var mediator = sp.GetRequiredService<IMediator>();
                var taskData = await mediator.Send(GetCreateTaskCommand());

                var rsp = await client.PutAsJsonAsync("/TaskTracker/update-task-priority", new UpdateWorkAssignmentPriorityCommand { Id = taskData.Id, NewPriority = WorkAssignmentPriority.Medium });
                var data = await rsp.Content.ReadFromJsonAsync<UpdateWorkAssignmentPriorityCommandResponse>();
                data.Should().NotBeNull();
                data.Success.Should().BeTrue();

                await mediator.Send(new DeleteWorkAssignmentCommand { Id = taskData.Id });
            });
        }

        [Fact]
        public async Task UpdateWorkAssignmentAuthor()
        {
            var client = await CreateClientWithJwt();

            await DoWorkAsync(async sp =>
            {
                var mediator = sp.GetRequiredService<IMediator>();
                var taskData = await mediator.Send(GetCreateTaskCommand());

                var rsp = await client.PutAsJsonAsync("/TaskTracker/update-task-author", new UpdateWorkAssignmentAuthorCommand { Id = taskData.Id, NewAuthor = "Sergey" });
                var data = await rsp.Content.ReadFromJsonAsync<UpdateWorkAssignmentAuthorCommandResponse>();
                data.Should().NotBeNull();
                data.Success.Should().BeTrue();

                await mediator.Send(new DeleteWorkAssignmentCommand { Id = taskData.Id });
            });
        }


        [Theory]
        [InlineData(null, "Ivan")]
        [InlineData("", "Ivan")]
        [InlineData("Petya", "Ivan")]
        [InlineData("Vasya", "")]
        [InlineData("Vasya", null)]
        public async Task UpdateWorkAssignmentWorker(string? oldWorker, string? newWorker)
        {
            var client = await CreateClientWithJwt();

            await DoWorkAsync(async sp =>
            {
                var mediator = sp.GetRequiredService<IMediator>();
                var taskData = await mediator.Send(GetCreateTaskCommand(worker: oldWorker));

                var rsp = await client.PutAsJsonAsync("/TaskTracker/update-task-worker", new UpdateWorkAssignmentWorkerCommand { Id = taskData.Id, NewWorker = newWorker });
                var data = await rsp.Content.ReadFromJsonAsync<UpdateWorkAssignmentWorkerCommandResponse>();
                data.Should().NotBeNull();
                data.Success.Should().BeTrue();

                await mediator.Send(new DeleteWorkAssignmentCommand { Id = taskData.Id });
            });
        }

        [Fact]
        public async Task AddSubWorkAssignment()
        {
            var client = await CreateClientWithJwt();

            await DoWorkAsync(async sp =>
            {
                var mediator = sp.GetRequiredService<IMediator>();
                var taskData1 = await mediator.Send(GetCreateTaskCommand(title: "Master task #1"));
                var taskData2 = await mediator.Send(GetCreateTaskCommand(title: "Sub task #1-1"));

                var rsp = await client.PutAsJsonAsync("/TaskTracker/add-sub-task", new AddSubWorkAssignmentCommand { WorkAssignmentId = taskData1.Id, SubWorkAssignmentId = taskData2.Id });
                var data = await rsp.Content.ReadFromJsonAsync<AddSubWorkAssignmentCommandResponse>();
                data.Should().NotBeNull();
                data.Success.Should().BeTrue();

                await mediator.Send(new DeleteWorkAssignmentCommand { Id = taskData2.Id });
                await mediator.Send(new DeleteWorkAssignmentCommand { Id = taskData1.Id });
            });
        }

        [Fact]
        public async Task ClearHeadWorkAssignment()
        {
            var client = await CreateClientWithJwt();

            await DoWorkAsync(async sp =>
            {
                var mediator = sp.GetRequiredService<IMediator>();
                var taskData1 = await mediator.Send(GetCreateTaskCommand(title: "Master task #1"));
                var taskData2 = await mediator.Send(GetCreateTaskCommand(title: "Sub task #1-1"));
                await mediator.Send(new AddSubWorkAssignmentCommand { WorkAssignmentId = taskData1.Id, SubWorkAssignmentId = taskData2.Id });

                var rsp = await client.PutAsJsonAsync("/TaskTracker/clear-master-task", new ClearHeadWorkAssignmentCommand { Id = taskData2.Id });
                var data = await rsp.Content.ReadFromJsonAsync<ClearHeadWorkAssignmentCommandResponse>();
                data.Should().NotBeNull();
                data.Success.Should().BeTrue();

                await mediator.Send(new DeleteWorkAssignmentCommand { Id = taskData2.Id });
                await mediator.Send(new DeleteWorkAssignmentCommand { Id = taskData1.Id });
            });
        }

        [Fact]
        public async Task SetRelation()
        {
            var client = await CreateClientWithJwt();

            await DoWorkAsync(async sp =>
            {
                var mediator = sp.GetRequiredService<IMediator>();
                var taskData1 = await mediator.Send(GetCreateTaskCommand(title: "Task #1"));
                var taskData2 = await mediator.Send(GetCreateTaskCommand(title: "Task #2"));

                var rsp = await client.PutAsJsonAsync("/TaskTracker/set-relation", new AddWorkAssignmentRelationCommand { Relation = WorkAssignmentRelationType.RelativeTo, SourceId = taskData1.Id, TargetId = taskData2.Id });
                var data = await rsp.Content.ReadFromJsonAsync<AddWorkAssignmentRelationCommandResponse>();
                data.Should().NotBeNull();
                data.Success.Should().BeTrue();
                
                await mediator.Send(new DeleteWorkAssignmentCommand { Id = taskData2.Id });
                await mediator.Send(new DeleteWorkAssignmentCommand { Id = taskData1.Id });
            });
        }

        [Fact]
        public async Task RemoveRelation()
        {
            var client = await CreateClientWithJwt();

            await DoWorkAsync(async sp =>
            {
                var mediator = sp.GetRequiredService<IMediator>();
                var taskData1 = await mediator.Send(GetCreateTaskCommand(title: "Task #1"));
                var taskData2 = await mediator.Send(GetCreateTaskCommand(title: "Task #2"));
                await mediator.Send(new AddWorkAssignmentRelationCommand { Relation = WorkAssignmentRelationType.RelativeTo, SourceId = taskData1.Id, TargetId = taskData2.Id });

                var rsp = await client.PutAsJsonAsync("/TaskTracker/remove-relation", new RemoveWorkAssignmentRelationCommand { Relation = WorkAssignmentRelationType.RelativeTo, SourceId = taskData1.Id, TargetId = taskData2.Id });
                var data = await rsp.Content.ReadFromJsonAsync<RemoveWorkAssignmentRelationCommandResponse>();
                data.Should().NotBeNull();
                data.Success.Should().BeTrue();

                await mediator.Send(new DeleteWorkAssignmentCommand { Id = taskData2.Id });
                await mediator.Send(new DeleteWorkAssignmentCommand { Id = taskData1.Id });
            });
        }

        [Fact]
        public async Task Get_without_subs_relations()
        {
            var client = await CreateClientWithJwt();

            await DoWorkAsync(async sp =>
            {
                var mediator = sp.GetRequiredService<IMediator>();
                var task1 = GetCreateTaskCommand(title: "Task #1");
                var taskData1 = await mediator.Send(task1);

                var data = await client.GetFromJsonAsync<GetWorkAssignmentByIdQueryResponse>($"/TaskTracker/get-task?id={taskData1.Id}", _jsonOptions);
                data.Should().NotBeNull();

                var taskInfo = data.TaskInfo!;
                taskInfo.Title.Should().Be(task1.Title);
                taskInfo.SubAssignments.Should().BeNullOrEmpty();
                taskInfo.OutRelations.Should().BeNullOrEmpty();
                taskInfo.InRelations.Should().BeNullOrEmpty();

                await mediator.Send(new DeleteWorkAssignmentCommand { Id = taskData1.Id });
            });
        }

        [Fact]
        public async Task Get_with_subs_relations()
        {
            var client = await CreateClientWithJwt();

            await DoWorkAsync(async sp =>
            {
                var mediator = sp.GetRequiredService<IMediator>();
                var task1 = GetCreateTaskCommand(title: "Task #1");
                var task2 = GetCreateTaskCommand(title: "Task #2");
                var task3 = GetCreateTaskCommand(title: "Sub Task #3");
                var taskData1 = await mediator.Send(task1);
                var taskData2 = await mediator.Send(task2);
                var taskData3 = await mediator.Send(task3);
                await mediator.Send(new AddWorkAssignmentRelationCommand { Relation = WorkAssignmentRelationType.RelativeTo, SourceId = taskData1.Id, TargetId = taskData2.Id });
                await mediator.Send(new AddSubWorkAssignmentCommand { WorkAssignmentId = taskData1.Id, SubWorkAssignmentId = taskData3.Id });

                var data = await client.GetFromJsonAsync<GetWorkAssignmentByIdQueryResponse>($"/TaskTracker/get-task?id={taskData1.Id}", _jsonOptions);
                data.Should().NotBeNull();

                var taskInfo = data.TaskInfo!;
                taskInfo.Title.Should().Be(task1.Title);
                taskInfo.SubAssignments.Should().NotBeNullOrEmpty();
                taskInfo.OutRelations.Should().NotBeNullOrEmpty();
                taskInfo.InRelations.Should().NotBeNullOrEmpty();

                await mediator.Send(new DeleteWorkAssignmentCommand { Id = taskData3.Id });
                await mediator.Send(new DeleteWorkAssignmentCommand { Id = taskData2.Id });
                await mediator.Send(new DeleteWorkAssignmentCommand { Id = taskData1.Id });
            });
        }

        private async Task DoWorkAsync(Func<IServiceProvider, Task> work)
        {
            using (var scope = _factory.Services.CreateScope())
            {
                await work(scope.ServiceProvider);
            }
        }

        private static CreateWorkAssignmentCommand GetCreateTaskCommand(
            string title = "Test task #1",
            string author = "Ivan",
            string? worker = null,
            WorkAssignmentStatus status = WorkAssignmentStatus.New,
            WorkAssignmentPriority priority = WorkAssignmentPriority.Low)
        {
            return new CreateWorkAssignmentCommand
            {
                Title = title,
                Author = author,
                Worker = worker,
                Status = status,
                Priority = priority
            };
        }

        private async Task<HttpClient> CreateClientWithJwt()
        {
            var client = _factory.CreateClient();
            var bearerToken = await GetAccessTokenAsync(client);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
            return client;
        }

        private static Task<string> GetAccessTokenAsync(HttpClient client)
        {
            return client.GetStringAsync("/JwtGenerator/gen-token?userId=test&username=test");
        }
    }
}