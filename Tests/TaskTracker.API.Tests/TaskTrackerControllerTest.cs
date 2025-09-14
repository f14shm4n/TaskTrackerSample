using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
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
        private const int InvalidId = int.MaxValue;
        private readonly WebApplicationFactory<Program> _factory;
        private readonly JsonSerializerOptions _jsonOptions = new(JsonSerializerDefaults.Web)
        {
            Converters = { new JsonStringEnumConverter() }
        };

        public TaskTrackerControllerTest(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        #region Create

        [Fact]
        public async Task CreateWorkAssignment()
        {
            await DoWorkAsync(async (client, sp) =>
            {
                var payload = (await PostAsJsonAsync<CreateWorkAssignmentCommand, ApiResponseBase<WorkAssignmentDTO>>(
                    client,
                    $"/{WorkAssignmentController.RootRoute}",
                    CreateTaskCreationCommand(),
                    HttpStatusCode.OK))
                ?.Payload;
                payload.Should().NotBeNull();

                await RemoveTaskListAsync(sp.GetRequiredService<IMediator>(), payload.Id);
            });
        }

        #endregion

        #region Delete

        [Fact]
        public async Task DeleteWorkAssignment_should_ok()
        {
            await DoWorkAsync(async (client, sp) =>
            {
                var id = (await CreateTaskListAsync(1, sp.GetRequiredService<IMediator>())).First();
                var rsp = await DeleteAsync<ApiResponseBase>(client, $"/{WorkAssignmentController.RootRoute}/{id}", HttpStatusCode.OK);
                rsp.Should().NotBeNull();
                rsp.Success.Should().BeTrue();
            });
        }

        [Fact]
        public async Task DeleteWorkAssignment_should_not_found()
        {
            await DoWorkAsync(async (client, sp) =>
            {
                var rsp = await DeleteAsync<ApiResponseBase>(client, $"/{WorkAssignmentController.RootRoute}/{InvalidId}", HttpStatusCode.NotFound);
                rsp.Should().NotBeNull();
                rsp.Success.Should().BeFalse();
            });
        }

        #endregion

        #region UpdateStatus

        [Fact]
        public async Task UpdateWorkAssignmentStatus_should_ok()
        {
            await DoWorkAsync(async (client, sp) =>
            {
                var mediator = sp.GetRequiredService<IMediator>();
                var id = (await CreateTaskListAsync(1, mediator)).First();

                var rsp = await PutAsync<ApiResponseBase>(client, $"/{WorkAssignmentController.RootRoute}/{id}/status/{WorkAssignmentStatus.InProgress}", HttpStatusCode.OK);
                rsp.Should().NotBeNull();
                rsp.Success.Should().BeTrue();

                await RemoveTaskListAsync(mediator, id);
            });
        }

        [Fact]
        public async Task UpdateWorkAssignmentStatus_should_notFound()
        {
            await DoWorkAsync(async (client, sp) =>
            {
                var rsp = await PutAsync<ApiResponseBase>(client, $"/{WorkAssignmentController.RootRoute}/{InvalidId}/status/{WorkAssignmentStatus.InProgress}", HttpStatusCode.NotFound);
                rsp.Should().NotBeNull();
                rsp.Success.Should().BeFalse();
            });
        }

        #endregion

        #region UpdatePriority

        [Fact]
        public async Task UpdateWorkAssignmentPriority_should_ok()
        {
            await DoWorkAsync(async (client, sp) =>
            {
                var mediator = sp.GetRequiredService<IMediator>();
                var id = (await CreateTaskListAsync(1, mediator)).First();

                var rsp = await PutAsync<ApiResponseBase>(client, $"/{WorkAssignmentController.RootRoute}/{id}/priority/{WorkAssignmentPriority.Medium}", HttpStatusCode.OK);
                rsp.Should().NotBeNull();
                rsp.Success.Should().BeTrue();

                await RemoveTaskListAsync(mediator, id);
            });
        }

        [Fact]
        public async Task UpdateWorkAssignmentPriority_should_notFound()
        {
            await DoWorkAsync(async (client, sp) =>
            {
                var rsp = await PutAsync<ApiResponseBase>(client, $"/{WorkAssignmentController.RootRoute}/{InvalidId}/priority/{WorkAssignmentPriority.Medium}", HttpStatusCode.NotFound);
                rsp.Should().NotBeNull();
                rsp.Success.Should().BeFalse();
            });
        }

        #endregion

        #region UpdateAuthor

        [Fact]
        public async Task UpdateWorkAssignmentAuthor_should_ok()
        {
            await DoWorkAsync(async (client, sp) =>
            {
                var mediator = sp.GetRequiredService<IMediator>();
                var id = (await CreateTaskListAsync(1, mediator)).First();

                var rsp = await PutAsync<ApiResponseBase>(client, $"/{WorkAssignmentController.RootRoute}/{id}/author/Sergey", HttpStatusCode.OK);
                rsp.Should().NotBeNull();
                rsp.Success.Should().BeTrue();

                await RemoveTaskListAsync(mediator, id);
            });
        }

        [Fact]
        public async Task UpdateWorkAssignmentAuthor_should_notFound()
        {
            await DoWorkAsync(async (client, sp) =>
            {
                var rsp = await PutAsync<ApiResponseBase>(client, $"/{WorkAssignmentController.RootRoute}/{InvalidId}/author/Sergey", HttpStatusCode.NotFound);
                rsp.Should().NotBeNull();
                rsp.Success.Should().BeFalse();
            });
        }

        #endregion

        #region UpdateWorker

        [Theory]
        [InlineData(null, "Ivan")]
        [InlineData("", "Ivan")]
        [InlineData("Petya", "Ivan")]
        public async Task UpdateWorkAssignmentWorker_should_ok(string? oldWorker, string? newWorker)
        {
            await DoWorkAsync(async (client, sp) =>
            {
                var mediator = sp.GetRequiredService<IMediator>();
                var id = (await mediator.Send(CreateTaskCreationCommand(worker: oldWorker))).Payload!.Id;

                var rsp = await PutAsync<ApiResponseBase>(client, $"/{WorkAssignmentController.RootRoute}/{id}/worker/{newWorker}", HttpStatusCode.OK);
                rsp.Should().NotBeNull();
                rsp.Success.Should().BeTrue();

                await RemoveTaskListAsync(mediator, id);
            });
        }

        [Fact]
        public async Task UpdateWorkAssignmentWorker_should_notFound()
        {
            await DoWorkAsync(async (client, sp) =>
            {
                var rsp = await PutAsync<ApiResponseBase>(client, $"/{WorkAssignmentController.RootRoute}/{InvalidId}/worker/Ivan", HttpStatusCode.NotFound);
                rsp.Should().NotBeNull();
                rsp.Success.Should().BeFalse();
            });
        }

        [Fact]
        public async Task RemoveWorkAssignmentWorker_should_ok()
        {
            await DoWorkAsync(async (client, sp) =>
            {
                var mediator = sp.GetRequiredService<IMediator>();
                var id = (await mediator.Send(CreateTaskCreationCommand(worker: "Vasya"))).Payload!.Id;

                var rsp = await PutAsync<ApiResponseBase>(client, $"/{WorkAssignmentController.RootRoute}/{id}/worker/unset", HttpStatusCode.OK);
                rsp.Should().NotBeNull();
                rsp.Success.Should().BeTrue();

                await RemoveTaskListAsync(mediator, id);
            });
        }

        [Fact]
        public async Task RemoveWorkAssignmentWorker_should_notFound()
        {
            await DoWorkAsync(async (client, sp) =>
            {
                var rsp = await PutAsync<ApiResponseBase>(client, $"/{WorkAssignmentController.RootRoute}/{InvalidId}/worker/unset", HttpStatusCode.NotFound);
                rsp.Should().NotBeNull();
                rsp.Success.Should().BeFalse();
            });
        }

        #endregion

        #region AddSubTask

        [Fact]
        public async Task AddSubWorkAssignment_should_ok()
        {
            await DoWorkAsync(async (client, sp) =>
            {
                var mediator = sp.GetRequiredService<IMediator>();
                var id1 = (await mediator.Send(CreateTaskCreationCommand(title: "Master task #1"))).Payload!.Id;
                var id2 = (await mediator.Send(CreateTaskCreationCommand(title: "Sub task #1-1"))).Payload!.Id;

                var rsp = await PutAsync<ApiResponseBase>(client, $"/{WorkAssignmentController.RootRoute}/{id1}/nesting/{id2}", HttpStatusCode.OK);
                rsp.Should().NotBeNull();
                rsp.Success.Should().BeTrue();

                await RemoveTaskListAsync(mediator, id1, id2);
            });
        }

        [Fact]
        public async Task AddSubWorkAssignment_should_ok_alreadySubtask()
        {
            await DoWorkAsync(async (client, sp) =>
            {
                var mediator = sp.GetRequiredService<IMediator>();
                var id1 = (await mediator.Send(CreateTaskCreationCommand(title: "Master task #1"))).Payload!.Id;
                var id2 = (await mediator.Send(CreateTaskCreationCommand(title: "Sub task #1-1"))).Payload!.Id;

                await PutAsync<ApiResponseBase>(client, $"/{WorkAssignmentController.RootRoute}/{id1}/nesting/{id2}", HttpStatusCode.OK);
                var rsp = await PutAsync<ApiResponseBase>(client, $"/{WorkAssignmentController.RootRoute}/{id1}/nesting/{id2}", HttpStatusCode.OK);
                rsp.Should().NotBeNull();
                rsp.Success.Should().BeTrue();

                await RemoveTaskListAsync(mediator, id1, id2);
            });
        }

        [Fact]
        public async Task AddSubWorkAssignment_should_badRequest_sameIds()
        {
            await DoWorkAsync(async (client, sp) =>
            {
                var rsp = await PutAsync<ApiResponseBase>(client, $"/{WorkAssignmentController.RootRoute}/1/nesting/1", HttpStatusCode.BadRequest);
                rsp.Should().NotBeNull();
                rsp.Success.Should().BeFalse();
            });
        }

        [Fact]
        public async Task AddSubWorkAssignment_should_notFound_headId()
        {
            await DoWorkAsync(async (client, sp) =>
            {
                var rsp = await PutAsync<ApiResponseBase>(client, $"/{WorkAssignmentController.RootRoute}/{InvalidId}/nesting/1", HttpStatusCode.NotFound);
                rsp.Should().NotBeNull();
                rsp.Success.Should().BeFalse();
            });
        }

        [Fact]
        public async Task AddSubWorkAssignment_should_notFound_subId()
        {
            await DoWorkAsync(async (client, sp) =>
            {
                var mediator = sp.GetRequiredService<IMediator>();
                var id1 = (await mediator.Send(CreateTaskCreationCommand(title: "Master task #1"))).Payload!.Id;
                var rsp = await PutAsync<ApiResponseBase>(client, $"/{WorkAssignmentController.RootRoute}/{id1}/nesting/{InvalidId}", HttpStatusCode.NotFound);
                rsp.Should().NotBeNull();
                rsp.Success.Should().BeFalse();

                await RemoveTaskListAsync(mediator, id1);
            });
        }

        #endregion

        #region ClearRootTask

        [Fact]
        public async Task ClearHeadWorkAssignment_should_ok()
        {
            await DoWorkAsync(async (client, sp) =>
            {
                var mediator = sp.GetRequiredService<IMediator>();
                var id1 = (await mediator.Send(CreateTaskCreationCommand(title: "Master task #1"))).Payload!.Id;
                var id2 = (await mediator.Send(CreateTaskCreationCommand(title: "Sub task #1-1"))).Payload!.Id;
                await mediator.Send(new AddSubWorkAssignmentCommand { WorkAssignmentId = id1, SubWorkAssignmentId = id2 });

                var rsp = await PutAsync<ApiResponseBase>(client, $"/{WorkAssignmentController.RootRoute}/{id2}/unnesting", HttpStatusCode.OK);
                rsp.Should().NotBeNull();
                rsp.Success.Should().BeTrue();

                await RemoveTaskListAsync(mediator, id1, id2);
            });
        }

        [Fact]
        public async Task ClearHeadWorkAssignment_should_notFound()
        {
            await DoWorkAsync(async (client, sp) =>
            {
                var rsp = await PutAsync<ApiResponseBase>(client, $"/{WorkAssignmentController.RootRoute}/{InvalidId}/unnesting", HttpStatusCode.NotFound);
                rsp.Should().NotBeNull();
                rsp.Success.Should().BeFalse();
            });
        }

        #endregion

        #region SetRelation

        [Fact]
        public async Task SetRelation_should_ok()
        {
            await DoWorkAsync(async (client, sp) =>
            {
                var mediator = sp.GetRequiredService<IMediator>();
                var id1 = (await mediator.Send(CreateTaskCreationCommand(title: "task #1"))).Payload!.Id;
                var id2 = (await mediator.Send(CreateTaskCreationCommand(title: "task #1-1"))).Payload!.Id;

                var rsp = await PutAsync<ApiResponseBase>(client, $"/{WorkAssignmentController.RootRoute}/{id1}/relate/{id2}/{WorkAssignmentRelationType.RelativeTo}", HttpStatusCode.OK);
                rsp.Should().NotBeNull();
                rsp.Success.Should().BeTrue();

                await RemoveTaskListAsync(mediator, id1, id2);
            });
        }

        [Fact]
        public async Task SetRelation_should_ok_hasRelation()
        {
            await DoWorkAsync(async (client, sp) =>
            {
                var mediator = sp.GetRequiredService<IMediator>();
                var id1 = (await mediator.Send(CreateTaskCreationCommand(title: "task #1"))).Payload!.Id;
                var id2 = (await mediator.Send(CreateTaskCreationCommand(title: "task #1-1"))).Payload!.Id;

                await PutAsync<ApiResponseBase>(client, $"/{WorkAssignmentController.RootRoute}/{id1}/relate/{id2}/{WorkAssignmentRelationType.RelativeTo}", HttpStatusCode.OK);
                var rsp = await PutAsync<ApiResponseBase>(client, $"/{WorkAssignmentController.RootRoute}/{id1}/relate/{id2}/{WorkAssignmentRelationType.RelativeTo}", HttpStatusCode.OK);
                rsp.Should().NotBeNull();
                rsp.Success.Should().BeTrue();

                await RemoveTaskListAsync(mediator, id1, id2);
            });
        }

        [Fact]
        public async Task SetRelation_should_badRequest_sameIds()
        {
            await DoWorkAsync(async (client, sp) =>
            {
                var rsp = await PutAsync<ApiResponseBase>(client, $"/{WorkAssignmentController.RootRoute}/1/relate/1/{WorkAssignmentRelationType.RelativeTo}", HttpStatusCode.BadRequest);
                rsp.Should().NotBeNull();
                rsp.Success.Should().BeFalse();
            });
        }

        [Fact]
        public async Task SetRelation_should_notFound_sourceId()
        {
            await DoWorkAsync(async (client, sp) =>
            {
                var rsp = await PutAsync<ApiResponseBase>(client, $"/{WorkAssignmentController.RootRoute}/{InvalidId}/relate/1/{WorkAssignmentRelationType.RelativeTo}", HttpStatusCode.NotFound);
                rsp.Should().NotBeNull();
                rsp.Success.Should().BeFalse();
            });
        }

        [Fact]
        public async Task SetRelation_should_notFound_targetId()
        {
            await DoWorkAsync(async (client, sp) =>
            {
                var mediator = sp.GetRequiredService<IMediator>();
                var id1 = (await mediator.Send(CreateTaskCreationCommand(title: "task #1"))).Payload!.Id;

                var rsp = await PutAsync<ApiResponseBase>(client, $"/{WorkAssignmentController.RootRoute}/{id1}/relate/1/{WorkAssignmentRelationType.RelativeTo}", HttpStatusCode.NotFound);
                rsp.Should().NotBeNull();
                rsp.Success.Should().BeFalse();

                await RemoveTaskListAsync(mediator, id1);
            });
        }

        #endregion

        #region RemoveRelation

        [Fact]
        public async Task RemoveRelation_should_ok()
        {
            await DoWorkAsync(async (client, sp) =>
            {
                var mediator = sp.GetRequiredService<IMediator>();
                var id1 = (await mediator.Send(CreateTaskCreationCommand(title: "task #1"))).Payload!.Id;
                var id2 = (await mediator.Send(CreateTaskCreationCommand(title: "task #1-1"))).Payload!.Id;
                await mediator.Send(new AddWorkAssignmentRelationCommand { Relation = WorkAssignmentRelationType.RelativeTo, SourceId = id1, TargetId = id2 });

                var rsp = await PutAsync<ApiResponseBase>(client, $"/{WorkAssignmentController.RootRoute}/{id1}/unrelate/{id2}/{WorkAssignmentRelationType.RelativeTo}", HttpStatusCode.OK);
                rsp.Should().NotBeNull();
                rsp.Success.Should().BeTrue();

                await RemoveTaskListAsync(mediator, id1, id2);
            });
        }

        [Fact]
        public async Task RemoveRelation_should_ok_noRelation()
        {
            await DoWorkAsync(async (client, sp) =>
            {
                var mediator = sp.GetRequiredService<IMediator>();
                var id1 = (await mediator.Send(CreateTaskCreationCommand(title: "task #1"))).Payload!.Id;
                var id2 = (await mediator.Send(CreateTaskCreationCommand(title: "task #1-1"))).Payload!.Id;

                var rsp = await PutAsync<ApiResponseBase>(client, $"/{WorkAssignmentController.RootRoute}/{id1}/unrelate/{id2}/{WorkAssignmentRelationType.RelativeTo}", HttpStatusCode.OK);
                rsp.Should().NotBeNull();
                rsp.Success.Should().BeTrue();

                await RemoveTaskListAsync(mediator, id1, id2);
            });
        }

        #endregion

        [Fact]
        public async Task Get_without_subs_relations()
        {
            await DoWorkAsync(async (client, sp) =>
            {
                var mediator = sp.GetRequiredService<IMediator>();
                var task1 = CreateTaskCreationCommand(title: "Task #1");
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
            await DoWorkAsync(async (client, sp) =>
            {
                var mediator = sp.GetRequiredService<IMediator>();
                var task1 = CreateTaskCreationCommand(title: "Task #1");
                var task2 = CreateTaskCreationCommand(title: "Task #2");
                var task3 = CreateTaskCreationCommand(title: "Sub Task #3");
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
            await DoWorkAsync(async (client, sp) =>
            {
                var mediator = sp.GetRequiredService<IMediator>();
                var taskIds = await CreateTaskListAsync(taskCount, mediator);
                var data = await client.GetFromJsonAsync<ApiResponseBase<List<WorkAssignmentDTO>>>($"/{WorkAssignmentController.RootRoute}/list?embed=false", _jsonOptions);
                data.Should().NotBeNull();
                data.Payload.Should().HaveCountGreaterThanOrEqualTo(taskCount);

                await RemoveTaskListAsync(mediator, taskIds.ToArray());
            });
        }

        #region Utils

        private async Task DoWorkAsync(Func<HttpClient, IServiceProvider, Task> work)
        {
            var client = await CreateClientWithJwt();
            using (var scope = _factory.Services.CreateScope())
            {
                await work(client, scope.ServiceProvider);
            }
        }

        #region HttpClient

        private async Task<TPayload?> PostAsJsonAsync<TRequest, TPayload>(HttpClient client, string requestUri, TRequest request, HttpStatusCode expectedStatusCode)
        {
            var rsp = await client.PostAsJsonAsync(requestUri, request);
            rsp.StatusCode.Should().Be(expectedStatusCode);
            return await rsp.Content.ReadFromJsonAsync<TPayload>(_jsonOptions);
        }

        private async Task<TPayload?> DeleteAsync<TPayload>(HttpClient client, string requestUri, HttpStatusCode expectedStatusCode)
        {
            var rsp = await client.DeleteAsync(requestUri);
            rsp.StatusCode.Should().Be(expectedStatusCode);
            return await rsp.Content.ReadFromJsonAsync<TPayload>(_jsonOptions);
        }

        private async Task<TPayload?> PutAsync<TPayload>(HttpClient client, string requestUri, HttpStatusCode expectedStatusCode, HttpContent? content = null)
        {
            var rsp = await client.PutAsync(requestUri, content);
            rsp.StatusCode.Should().Be(expectedStatusCode);
            return await rsp.Content.ReadFromJsonAsync<TPayload>(_jsonOptions);
        }

        private async Task<HttpClient> CreateClientWithJwt()
        {
            var client = _factory.CreateClient();
            var bearerToken = await GetAccessTokenAsync(client);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
            return client;
        }

        #endregion

        private static CreateWorkAssignmentCommand CreateTaskCreationCommand(
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

        private static Task<string> GetAccessTokenAsync(HttpClient client)
        {
            return client.GetStringAsync("/JwtGenerator/generate?userId=test&username=test");
        }

        private static async Task<IEnumerable<int>> CreateTaskListAsync(int count, IMediator mediator)
        {
            var taskIds = new List<int>();
            for (var i = 0; i < count; i++)
            {
                taskIds.Add((await mediator.Send(CreateTaskCreationCommand(title: $"Task #{i + 1}"))).Payload!.Id);
            }
            return taskIds;
        }

        private static async Task RemoveTaskListAsync(IMediator mediator, params int[] ids)
        {
            foreach (var id in ids)
            {
                await mediator.Send(new DeleteWorkAssignmentCommand { Id = id });
            }
        }

        #endregion
    }
}