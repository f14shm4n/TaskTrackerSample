using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using TaskTracker.API.Application;
using TaskTracker.API.Application.Commands;
using TaskTracker.API.Application.Dto;
using TaskTracker.API.Controllers;
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
                var rsp = await client.PostAsJsonAsync($"/{WorkAssignmentController.RootRoute}", GetCreateTaskCommand());
                var rspData = await rsp.Content.ReadFromJsonAsync<ApiResponseBase<WorkAssignmentDTO>>(_jsonOptions);
                rspData.Should().NotBeNull();
                rspData.Payload.Should().NotBeNull();

                await sp.GetRequiredService<IMediator>().Send(new DeleteWorkAssignmentCommand { Id = rspData.Payload.Id });
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

                var rsp = await client.DeleteAsync($"/{WorkAssignmentController.RootRoute}/{taskData.Payload!.Id}");
                var rspData = await rsp.Content.ReadFromJsonAsync<ApiResponseBase>();
                rspData.Should().NotBeNull();
                rspData.Success.Should().BeTrue();
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

                var rsp = await client.PutAsync($"/{WorkAssignmentController.RootRoute}/{taskData.Payload!.Id}/status/{WorkAssignmentStatus.InProgress}", null);
                var rspData = await rsp.Content.ReadFromJsonAsync<ApiResponseBase>();
                rspData.Should().NotBeNull();
                rspData.Success.Should().BeTrue();

                await mediator.Send(new DeleteWorkAssignmentCommand { Id = taskData.Payload!.Id });
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

                var rsp = await client.PutAsync($"/{WorkAssignmentController.RootRoute}/{taskData.Payload!.Id}/priority/{WorkAssignmentPriority.Medium}", null);
                var rspData = await rsp.Content.ReadFromJsonAsync<ApiResponseBase>();
                rspData.Should().NotBeNull();
                rspData.Success.Should().BeTrue();

                await mediator.Send(new DeleteWorkAssignmentCommand { Id = taskData.Payload!.Id });
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

                var rsp = await client.PutAsync($"/{WorkAssignmentController.RootRoute}/{taskData.Payload!.Id}/author/Sergey", null);
                var rspData = await rsp.Content.ReadFromJsonAsync<ApiResponseBase>();
                rspData.Should().NotBeNull();
                rspData.Success.Should().BeTrue();

                await mediator.Send(new DeleteWorkAssignmentCommand { Id = taskData.Payload!.Id });
            });
        }


        [Theory]
        [InlineData(null, "Ivan")]
        [InlineData("", "Ivan")]
        [InlineData("Petya", "Ivan")]
        public async Task UpdateWorkAssignmentWorker(string? oldWorker, string? newWorker)
        {
            var client = await CreateClientWithJwt();

            await DoWorkAsync(async sp =>
            {
                var mediator = sp.GetRequiredService<IMediator>();
                var taskData = await mediator.Send(GetCreateTaskCommand(worker: oldWorker));

                var rsp = await client.PutAsync($"/{WorkAssignmentController.RootRoute}/{taskData.Payload!.Id}/worker/{newWorker}", null);
                var rspData = await rsp.Content.ReadFromJsonAsync<ApiResponseBase>();
                rspData.Should().NotBeNull();
                rspData.Success.Should().BeTrue();

                await mediator.Send(new DeleteWorkAssignmentCommand { Id = taskData.Payload!.Id });
            });
        }

        [Fact]
        public async Task RemoveWorkAssignmentWorker()
        {
            var client = await CreateClientWithJwt();

            await DoWorkAsync(async sp =>
            {
                var mediator = sp.GetRequiredService<IMediator>();
                var taskData = await mediator.Send(GetCreateTaskCommand(worker: "Vasya"));

                var rsp = await client.PutAsync($"/{WorkAssignmentController.RootRoute}/{taskData.Payload!.Id}/worker/unset", null);
                var rspData = await rsp.Content.ReadFromJsonAsync<ApiResponseBase>();
                rspData.Should().NotBeNull();
                rspData.Success.Should().BeTrue();

                await mediator.Send(new DeleteWorkAssignmentCommand { Id = taskData.Payload!.Id });
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

                var rsp = await client.PutAsync($"/{WorkAssignmentController.RootRoute}/{taskData1.Payload!.Id}/nesting/{taskData2.Payload!.Id}", null);
                var rspData = await rsp.Content.ReadFromJsonAsync<ApiResponseBase>();
                rspData.Should().NotBeNull();
                rspData.Success.Should().BeTrue();

                await mediator.Send(new DeleteWorkAssignmentCommand { Id = taskData2.Payload!.Id });
                await mediator.Send(new DeleteWorkAssignmentCommand { Id = taskData1.Payload!.Id });
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
                await mediator.Send(new AddSubWorkAssignmentCommand { WorkAssignmentId = taskData1.Payload!.Id, SubWorkAssignmentId = taskData2.Payload!.Id });

                var rsp = await client.PutAsync($"/{WorkAssignmentController.RootRoute}/{taskData2.Payload!.Id}/unnesting", null);
                var rspData = await rsp.Content.ReadFromJsonAsync<ApiResponseBase>();
                rspData.Should().NotBeNull();
                rspData.Success.Should().BeTrue();

                await mediator.Send(new DeleteWorkAssignmentCommand { Id = taskData2.Payload!.Id });
                await mediator.Send(new DeleteWorkAssignmentCommand { Id = taskData1.Payload!.Id });
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

                var rsp = await client.PutAsync($"/{WorkAssignmentController.RootRoute}/{taskData1.Payload!.Id}/relate/{taskData2.Payload!.Id}/{WorkAssignmentRelationType.RelativeTo}", null);
                var rspData = await rsp.Content.ReadFromJsonAsync<ApiResponseBase>();
                rspData.Should().NotBeNull();
                rspData.Success.Should().BeTrue();

                await mediator.Send(new DeleteWorkAssignmentCommand { Id = taskData2.Payload!.Id });
                await mediator.Send(new DeleteWorkAssignmentCommand { Id = taskData1.Payload!.Id });
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
                await mediator.Send(new AddWorkAssignmentRelationCommand { Relation = WorkAssignmentRelationType.RelativeTo, SourceId = taskData1.Payload!.Id, TargetId = taskData2.Payload!.Id });

                var rsp = await client.PutAsync($"/{WorkAssignmentController.RootRoute}/{taskData1.Payload!.Id}/unrelate/{taskData2.Payload!.Id}/{WorkAssignmentRelationType.RelativeTo}", null);
                var rspData = await rsp.Content.ReadFromJsonAsync<ApiResponseBase>();
                rspData.Should().NotBeNull();
                rspData.Success.Should().BeTrue();

                await mediator.Send(new DeleteWorkAssignmentCommand { Id = taskData2.Payload!.Id });
                await mediator.Send(new DeleteWorkAssignmentCommand { Id = taskData1.Payload!.Id });
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

                var rspData = await client.GetFromJsonAsync<ApiResponseBase<WorkAssignmentDTO>>($"/{WorkAssignmentController.RootRoute}/{taskData1.Payload!.Id}", _jsonOptions);
                rspData.Should().NotBeNull();

                var taskDto = rspData.Payload!;
                taskDto.Title.Should().Be(task1.Title);
                taskDto.SubAssignments.Should().BeNullOrEmpty();
                taskDto.OutRelations.Should().BeNullOrEmpty();
                taskDto.InRelations.Should().BeNullOrEmpty();

                await mediator.Send(new DeleteWorkAssignmentCommand { Id = taskData1.Payload!.Id });
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
                await mediator.Send(new AddWorkAssignmentRelationCommand { Relation = WorkAssignmentRelationType.RelativeTo, SourceId = taskData1.Payload!.Id, TargetId = taskData2.Payload!.Id });
                await mediator.Send(new AddSubWorkAssignmentCommand { WorkAssignmentId = taskData1.Payload!.Id, SubWorkAssignmentId = taskData3.Payload!.Id });

                var rspData = await client.GetFromJsonAsync<ApiResponseBase<WorkAssignmentDTO>>($"/{WorkAssignmentController.RootRoute}/{taskData1.Payload!.Id}", _jsonOptions);
                rspData.Should().NotBeNull();

                var taskDto = rspData.Payload!;
                taskDto.Title.Should().Be(task1.Title);
                taskDto.SubAssignments.Should().NotBeNullOrEmpty();
                taskDto.OutRelations.Should().NotBeNullOrEmpty();
                taskDto.InRelations.Should().NotBeNullOrEmpty();

                await mediator.Send(new DeleteWorkAssignmentCommand { Id = taskData3.Payload!.Id });
                await mediator.Send(new DeleteWorkAssignmentCommand { Id = taskData2.Payload!.Id });
                await mediator.Send(new DeleteWorkAssignmentCommand { Id = taskData1.Payload!.Id });
            });
        }

        [Fact]
        public async Task GetTasks_without_relations()
        {
            const int taskCount = 5;
            var client = await CreateClientWithJwt();

            await DoWorkAsync(async sp =>
            {
                var mediator = sp.GetRequiredService<IMediator>();
                var taskIds = await CreateTaskListAsync(taskCount, mediator);
                var data = await client.GetFromJsonAsync<ApiResponseBase<List<WorkAssignmentDTO>>>($"/{WorkAssignmentController.RootRoute}/list?embed=false", _jsonOptions);
                data.Should().NotBeNull();
                data.Payload.Should().HaveCountGreaterThanOrEqualTo(taskCount);

                await RemoveTaskListAsync(taskIds, mediator);
            });
        }

        #region Utils

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
            return client.GetStringAsync("/JwtGenerator/generate?userId=test&username=test");
        }

        private static async Task<IEnumerable<int>> CreateTaskListAsync(int count, IMediator mediator)
        {
            var taskIds = new List<int>();
            for (var i = 0; i < count; i++)
            {
                taskIds.Add((await mediator.Send(GetCreateTaskCommand(title: $"Task #{i + 1}"))).Payload!.Id);
            }
            return taskIds;
        }

        private static async Task RemoveTaskListAsync(IEnumerable<int> ids, IMediator mediator)
        {
            foreach (var id in ids)
            {
                await mediator.Send(new DeleteWorkAssignmentCommand { Id = id });
            }
        }

        #endregion
    }
}