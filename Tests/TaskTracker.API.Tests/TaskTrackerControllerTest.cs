using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
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
        public async Task Create_should_ok()
        {
            await DoWorkAsync(async (client, sp) =>
            {
                var payload = (await PostAsJsonAsync<CreateWorkAssignmentCommand, ApiResponseValue<WorkAssignmentDTO>>(
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
        public async Task Delete_should_ok()
        {
            await DoWorkAsync(async (client, sp) =>
            {
                var id = (await CreateTaskListAsync(1, sp.GetRequiredService<IMediator>())).First();
                await DeleteAsync<ApiResponseValue>(client, $"/{WorkAssignmentController.RootRoute}/{id}", HttpStatusCode.OK);
            });
        }

        [Fact]
        public async Task Delete_should_not_found()
        {
            await DoWorkAsync(async (client, sp) =>
            {
                await DeleteAsync<ProblemDetails>(client, $"/{WorkAssignmentController.RootRoute}/{InvalidId}", HttpStatusCode.NotFound);
            });
        }

        #endregion

        #region UpdateStatus

        [Fact]
        public async Task UpdateStatus_should_ok()
        {
            await DoWorkAsync(async (client, sp) =>
            {
                var mediator = sp.GetRequiredService<IMediator>();
                var id = (await CreateTaskListAsync(1, mediator)).First();

                try
                {
                    await PutAsync<ApiResponseValue>(client, $"/{WorkAssignmentController.RootRoute}/{id}/status/{WorkAssignmentStatus.InProgress}", HttpStatusCode.OK);
                }
                finally
                {
                    await RemoveTaskListAsync(mediator, id);
                }
            });
        }

        [Fact]
        public async Task UpdateStatus_should_notFound()
        {
            await DoWorkAsync(async (client, sp) =>
            {
                await PutAsync<ApiResponseValue>(client, $"/{WorkAssignmentController.RootRoute}/{InvalidId}/status/{WorkAssignmentStatus.InProgress}", HttpStatusCode.NotFound);
            });
        }

        #endregion

        #region UpdatePriority

        [Fact]
        public async Task UpdatePriority_should_ok()
        {
            await DoWorkAsync(async (client, sp) =>
            {
                var mediator = sp.GetRequiredService<IMediator>();
                var id = (await CreateTaskListAsync(1, mediator)).First();

                try
                {
                    await PutAsync<ApiResponseValue>(client, $"/{WorkAssignmentController.RootRoute}/{id}/priority/{WorkAssignmentPriority.Medium}", HttpStatusCode.OK);
                }
                finally
                {
                    await RemoveTaskListAsync(mediator, id);
                }
            });
        }

        [Fact]
        public async Task UpdatePriority_should_notFound()
        {
            await DoWorkAsync(async (client, sp) =>
            {
                await PutAsync<ApiResponseValue>(client, $"/{WorkAssignmentController.RootRoute}/{InvalidId}/priority/{WorkAssignmentPriority.Medium}", HttpStatusCode.NotFound);
            });
        }

        #endregion

        #region UpdateAuthor

        [Fact]
        public async Task UpdateAuthor_should_ok()
        {
            await DoWorkAsync(async (client, sp) =>
            {
                var mediator = sp.GetRequiredService<IMediator>();
                var id = (await CreateTaskListAsync(1, mediator)).First();

                try
                {
                    await PutAsync<ApiResponseValue>(client, $"/{WorkAssignmentController.RootRoute}/{id}/author/Sergey", HttpStatusCode.OK);
                }
                finally
                {
                    await RemoveTaskListAsync(mediator, id);
                }
            });
        }

        [Fact]
        public async Task UpdateAuthor_should_notFound()
        {
            await DoWorkAsync(async (client, sp) =>
            {
                await PutAsync<ApiResponseValue>(client, $"/{WorkAssignmentController.RootRoute}/{InvalidId}/author/Sergey", HttpStatusCode.NotFound);
            });
        }

        #endregion

        #region UpdateWorker

        [Theory]
        [InlineData(null, "Ivan")]
        [InlineData("", "Ivan")]
        [InlineData("Petya", "Ivan")]
        public async Task UpdateWorker_should_ok(string? oldWorker, string? newWorker)
        {
            await DoWorkAsync(async (client, sp) =>
            {
                var mediator = sp.GetRequiredService<IMediator>();
                var id = ((ApiResponseValue<WorkAssignmentDTO>)(await mediator.Send(CreateTaskCreationCommand(worker: oldWorker))).Value!).Payload!.Id;

                try
                {
                    await PutAsync<ApiResponseValue>(client, $"/{WorkAssignmentController.RootRoute}/{id}/worker/{newWorker}", HttpStatusCode.OK);
                }
                finally
                {
                    await RemoveTaskListAsync(mediator, id);
                }
            });
        }

        [Fact]
        public async Task UpdateWorker_should_notFound()
        {
            await DoWorkAsync(async (client, sp) =>
            {
                await PutAsync<ApiResponseValue>(client, $"/{WorkAssignmentController.RootRoute}/{InvalidId}/worker/Ivan", HttpStatusCode.NotFound);
            });
        }

        #endregion

        #region UnsetWorker

        [Fact]
        public async Task UnsetWorker_should_ok()
        {
            await DoWorkAsync(async (client, sp) =>
            {
                var mediator = sp.GetRequiredService<IMediator>();
                var id = ((ApiResponseValue<WorkAssignmentDTO>)(await mediator.Send(CreateTaskCreationCommand(worker: "Vasya"))).Value!).Payload!.Id;

                try
                {
                    await PutAsync<ApiResponseValue>(client, $"/{WorkAssignmentController.RootRoute}/{id}/worker/unset", HttpStatusCode.OK);
                }
                finally
                {
                    await RemoveTaskListAsync(mediator, id);
                }
            });
        }

        [Fact]
        public async Task UnsetWorker_should_notFound()
        {
            await DoWorkAsync(async (client, sp) =>
            {
                await PutAsync<ApiResponseValue>(client, $"/{WorkAssignmentController.RootRoute}/{InvalidId}/worker/unset", HttpStatusCode.NotFound);
            });
        }

        #endregion

        #region AddNesting

        [Fact]
        public async Task AddNesting_should_ok()
        {
            await DoWorkAsync(async (client, sp) =>
            {
                var mediator = sp.GetRequiredService<IMediator>();
                var id1 = ((ApiResponseValue<WorkAssignmentDTO>)(await mediator.Send(CreateTaskCreationCommand(title: "Master task #1"))).Value!).Payload!.Id;
                var id2 = ((ApiResponseValue<WorkAssignmentDTO>)(await mediator.Send(CreateTaskCreationCommand(title: "Sub task #1-1"))).Value!).Payload!.Id;

                try
                {
                    await PutAsync<ApiResponseValue>(client, $"/{WorkAssignmentController.RootRoute}/{id1}/nesting/{id2}", HttpStatusCode.OK);
                }
                finally
                {
                    await RemoveTaskListAsync(mediator, id1, id2);
                }
            });
        }

        [Fact]
        public async Task AddNesting_alreadySubtask_should_ok()
        {
            await DoWorkAsync(async (client, sp) =>
            {
                var mediator = sp.GetRequiredService<IMediator>();
                var id1 = ((ApiResponseValue<WorkAssignmentDTO>)(await mediator.Send(CreateTaskCreationCommand(title: "Master task #1"))).Value!).Payload!.Id;
                var id2 = ((ApiResponseValue<WorkAssignmentDTO>)(await mediator.Send(CreateTaskCreationCommand(title: "Sub task #1-1"))).Value!).Payload!.Id;

                try
                {
                    await PutAsync<ApiResponseValue>(client, $"/{WorkAssignmentController.RootRoute}/{id1}/nesting/{id2}", HttpStatusCode.OK);
                    await PutAsync<ApiResponseValue>(client, $"/{WorkAssignmentController.RootRoute}/{id1}/nesting/{id2}", HttpStatusCode.OK);
                }
                finally
                {
                    await RemoveTaskListAsync(mediator, id1, id2);
                }
            });
        }

        [Fact]
        public async Task AddNesting_sameIds_should_badRequest()
        {
            await DoWorkAsync(async (client, sp) =>
            {
                await PutAsync<ApiResponseValue>(client, $"/{WorkAssignmentController.RootRoute}/{InvalidId}/nesting/{InvalidId}", HttpStatusCode.BadRequest);
            });
        }

        [Fact]
        public async Task AddNesting_headId_should_notFound()
        {
            await DoWorkAsync(async (client, sp) =>
            {
                await PutAsync<ApiResponseValue>(client, $"/{WorkAssignmentController.RootRoute}/{InvalidId}/nesting/1", HttpStatusCode.NotFound);
            });
        }

        [Fact]
        public async Task AddNesting_subId_should_notFound()
        {
            await DoWorkAsync(async (client, sp) =>
            {
                var mediator = sp.GetRequiredService<IMediator>();
                var id1 = ((ApiResponseValue<WorkAssignmentDTO>)(await mediator.Send(CreateTaskCreationCommand(title: "Master task #1"))).Value!).Payload!.Id;

                try
                {
                    await PutAsync<ApiResponseValue>(client, $"/{WorkAssignmentController.RootRoute}/{id1}/nesting/{InvalidId}", HttpStatusCode.NotFound);
                }
                finally
                {
                    await RemoveTaskListAsync(mediator, id1);
                }
            });
        }

        #endregion

        #region RemoveNesting

        [Fact]
        public async Task RemoveNesting_should_ok()
        {
            await DoWorkAsync(async (client, sp) =>
            {
                var mediator = sp.GetRequiredService<IMediator>();
                var id1 = ((ApiResponseValue<WorkAssignmentDTO>)(await mediator.Send(CreateTaskCreationCommand(title: "Master task #1"))).Value!).Payload!.Id;
                var id2 = ((ApiResponseValue<WorkAssignmentDTO>)(await mediator.Send(CreateTaskCreationCommand(title: "Sub task #1-1"))).Value!).Payload!.Id;
                await mediator.Send(new AddSubWorkAssignmentCommand { WorkAssignmentId = id1, SubWorkAssignmentId = id2 });

                try
                {
                    await PutAsync<ApiResponseValue>(client, $"/{WorkAssignmentController.RootRoute}/{id2}/unnesting", HttpStatusCode.OK);
                }
                finally
                {
                    await RemoveTaskListAsync(mediator, id1, id2);
                }
            });
        }

        [Fact]
        public async Task RemoveNesting_should_notFound()
        {
            await DoWorkAsync(async (client, sp) =>
            {
                await PutAsync<ApiResponseValue>(client, $"/{WorkAssignmentController.RootRoute}/{InvalidId}/unnesting", HttpStatusCode.NotFound);
            });
        }

        #endregion

        #region Relate

        [Fact]
        public async Task Relate_should_ok()
        {
            await DoWorkAsync(async (client, sp) =>
            {
                var mediator = sp.GetRequiredService<IMediator>();
                var id1 = ((ApiResponseValue<WorkAssignmentDTO>)(await mediator.Send(CreateTaskCreationCommand(title: "task #1"))).Value!).Payload!.Id;
                var id2 = ((ApiResponseValue<WorkAssignmentDTO>)(await mediator.Send(CreateTaskCreationCommand(title: "task #1-1"))).Value!).Payload!.Id;

                try
                {
                    await PutAsync<ApiResponseValue>(client, $"/{WorkAssignmentController.RootRoute}/{id1}/relate/{id2}/{WorkAssignmentRelationType.RelativeTo}", HttpStatusCode.OK);
                }
                finally
                {
                    await RemoveTaskListAsync(mediator, id1, id2);
                }
            });
        }

        [Fact]
        public async Task Relate_hasRelation_should_ok()
        {
            await DoWorkAsync(async (client, sp) =>
            {
                var mediator = sp.GetRequiredService<IMediator>();
                var id1 = ((ApiResponseValue<WorkAssignmentDTO>)(await mediator.Send(CreateTaskCreationCommand(title: "task #1"))).Value!).Payload!.Id;
                var id2 = ((ApiResponseValue<WorkAssignmentDTO>)(await mediator.Send(CreateTaskCreationCommand(title: "task #1-1"))).Value!).Payload!.Id;

                try
                {
                    await PutAsync<ApiResponseValue>(client, $"/{WorkAssignmentController.RootRoute}/{id1}/relate/{id2}/{WorkAssignmentRelationType.RelativeTo}", HttpStatusCode.OK);
                    await PutAsync<ApiResponseValue>(client, $"/{WorkAssignmentController.RootRoute}/{id1}/relate/{id2}/{WorkAssignmentRelationType.RelativeTo}", HttpStatusCode.OK);
                }
                finally
                {
                    await RemoveTaskListAsync(mediator, id1, id2);
                }
            });
        }

        [Fact]
        public async Task Relate_sameIds_should_badRequest()
        {
            await DoWorkAsync(async (client, sp) =>
            {
                await PutAsync<ApiResponseValue>(client, $"/{WorkAssignmentController.RootRoute}/{InvalidId}/relate/{InvalidId}/{WorkAssignmentRelationType.RelativeTo}", HttpStatusCode.BadRequest);
            });
        }

        [Fact]
        public async Task Relate_sourceId_should_notFound()
        {
            await DoWorkAsync(async (client, sp) =>
            {
                await PutAsync<ApiResponseValue>(client, $"/{WorkAssignmentController.RootRoute}/{InvalidId}/relate/1/{WorkAssignmentRelationType.RelativeTo}", HttpStatusCode.NotFound);
            });
        }

        [Fact]
        public async Task Relate_targetId_should_notFound()
        {
            await DoWorkAsync(async (client, sp) =>
            {
                var mediator = sp.GetRequiredService<IMediator>();
                var id1 = ((ApiResponseValue<WorkAssignmentDTO>)(await mediator.Send(CreateTaskCreationCommand(title: "task #1"))).Value!).Payload!.Id;

                try
                {
                    await PutAsync<ApiResponseValue>(client, $"/{WorkAssignmentController.RootRoute}/{id1}/relate/{InvalidId}/{WorkAssignmentRelationType.RelativeTo}", HttpStatusCode.NotFound);
                }
                finally
                {
                    await RemoveTaskListAsync(mediator, id1);
                }
            });
        }

        #endregion

        #region Unrelate

        [Fact]
        public async Task Unrelate_should_ok()
        {
            await DoWorkAsync(async (client, sp) =>
            {
                var mediator = sp.GetRequiredService<IMediator>();
                var id1 = ((ApiResponseValue<WorkAssignmentDTO>)(await mediator.Send(CreateTaskCreationCommand(title: "task #1"))).Value!).Payload!.Id;
                var id2 = ((ApiResponseValue<WorkAssignmentDTO>)(await mediator.Send(CreateTaskCreationCommand(title: "task #1-1"))).Value!).Payload!.Id;
                await mediator.Send(new AddWorkAssignmentRelationCommand { Relation = WorkAssignmentRelationType.RelativeTo, SourceId = id1, TargetId = id2 });

                try
                {
                    await PutAsync<ApiResponseValue>(client, $"/{WorkAssignmentController.RootRoute}/{id1}/unrelate/{id2}/{WorkAssignmentRelationType.RelativeTo}", HttpStatusCode.OK);
                }
                finally
                {
                    await RemoveTaskListAsync(mediator, id1, id2);
                }
            });
        }

        [Fact]
        public async Task Unrelate_noRelation_should_ok()
        {
            await DoWorkAsync(async (client, sp) =>
            {
                var mediator = sp.GetRequiredService<IMediator>();
                var id1 = ((ApiResponseValue<WorkAssignmentDTO>)(await mediator.Send(CreateTaskCreationCommand(title: "task #1"))).Value!).Payload!.Id;
                var id2 = ((ApiResponseValue<WorkAssignmentDTO>)(await mediator.Send(CreateTaskCreationCommand(title: "task #1-1"))).Value!).Payload!.Id;

                try
                {
                    await PutAsync<ApiResponseValue>(client, $"/{WorkAssignmentController.RootRoute}/{id1}/unrelate/{id2}/{WorkAssignmentRelationType.RelativeTo}", HttpStatusCode.OK);
                }
                finally
                {
                    await RemoveTaskListAsync(mediator, id1, id2);
                }
            });
        }

        [Fact]
        public async Task Unrelate_sameIds_should_badRequest()
        {
            await DoWorkAsync(async (client, sp) =>
            {
                await PutAsync<ApiResponseValue>(client, $"/{WorkAssignmentController.RootRoute}/{InvalidId}/unrelate/{InvalidId}/{WorkAssignmentRelationType.RelativeTo}", HttpStatusCode.BadRequest);
            });
        }

        #endregion

        #region Get

        [Fact]
        public async Task Get_withoutEmbed_should_ok()
        {
            await DoWorkAsync(async (client, sp) =>
            {
                var mediator = sp.GetRequiredService<IMediator>();
                var t1 = ((ApiResponseValue<WorkAssignmentDTO>)(await mediator.Send(CreateTaskCreationCommand(title: "Task #1"))).Value!).Payload!;
                try
                {
                    var dto = (await client.GetFromJsonAsync<ApiResponseValue<WorkAssignmentDTO>>($"/{WorkAssignmentController.RootRoute}/{t1.Id}", _jsonOptions))?.Payload!;
                    dto.Title.Should().Be(t1.Title);
                    dto.SubAssignments.Should().BeNullOrEmpty();
                    dto.OutRelations.Should().BeNullOrEmpty();
                    dto.InRelations.Should().BeNullOrEmpty();
                }
                finally
                {
                    await RemoveTaskListAsync(mediator, t1.Id);
                }
            });
        }

        [Fact]
        public async Task Get_withEmbed_should_ok()
        {
            await DoWorkAsync(async (client, sp) =>
            {
                var mediator = sp.GetRequiredService<IMediator>();
                var t1 = ((ApiResponseValue<WorkAssignmentDTO>)(await mediator.Send(CreateTaskCreationCommand(title: "Task #1"))).Value!).Payload!;
                var t2 = ((ApiResponseValue<WorkAssignmentDTO>)(await mediator.Send(CreateTaskCreationCommand(title: "Task #2"))).Value!).Payload!;
                var t3 = ((ApiResponseValue<WorkAssignmentDTO>)(await mediator.Send(CreateTaskCreationCommand(title: "Sub Task #3"))).Value!).Payload!;
                await mediator.Send(new AddWorkAssignmentRelationCommand { Relation = WorkAssignmentRelationType.RelativeTo, SourceId = t1.Id, TargetId = t2.Id });
                await mediator.Send(new AddSubWorkAssignmentCommand { WorkAssignmentId = t1.Id, SubWorkAssignmentId = t3.Id });

                try
                {
                    var taskDto = (await client.GetFromJsonAsync<ApiResponseValue<WorkAssignmentDTO>>($"/{WorkAssignmentController.RootRoute}/{t1.Id}?embed=true", _jsonOptions))?.Payload!;
                    taskDto.Title.Should().Be(t1.Title);
                    taskDto.SubAssignments.Should().NotBeNullOrEmpty();
                    taskDto.OutRelations.Should().NotBeNullOrEmpty();
                    taskDto.InRelations.Should().NotBeNullOrEmpty();
                }
                finally
                {
                    await RemoveTaskListAsync(mediator, t1.Id, t2.Id, t3.Id);
                }
            });
        }

        #endregion

        #region GetList

        [Fact]
        public async Task GetTasks_withEmbed_cursor_limit_should_ok()
        {
            await DoWorkAsync(async (client, sp) =>
            {
                var mediator = sp.GetRequiredService<IMediator>();
                var t1 = ((ApiResponseValue<WorkAssignmentDTO>)(await mediator.Send(CreateTaskCreationCommand(title: "Task #1"))).Value!).Payload!;
                var t2 = ((ApiResponseValue<WorkAssignmentDTO>)(await mediator.Send(CreateTaskCreationCommand(title: "Sub Task #1-1"))).Value!).Payload!;
                await mediator.Send(new AddWorkAssignmentRelationCommand { Relation = WorkAssignmentRelationType.RelativeTo, SourceId = t1.Id, TargetId = t2.Id });
                await mediator.Send(new AddSubWorkAssignmentCommand { WorkAssignmentId = t1.Id, SubWorkAssignmentId = t2.Id });

                try
                {
                    var data = await client.GetFromJsonAsync<ApiResponseValue<List<WorkAssignmentDTO>>>($"/{WorkAssignmentController.RootRoute}/list?embed=true&cursor={t1.Id - 1}&limit=2", _jsonOptions);
                    data.Should().NotBeNull();
                    data.Payload.Should().HaveCount(2);
                    data.Payload.ForEach(x =>
                    {
                        if (x.Id == t1.Id)
                        {
                            x.SubAssignments.Should().HaveCount(1);
                            x.InRelations.Should().HaveCount(1);
                            x.OutRelations.Should().HaveCount(1);
                        }
                        else if (x.Id == t2.Id)
                        {
                            x.HeadAssignmentId.Should().NotBeNull();
                            x.HeadAssignmentId.Should().Be(t1.Id);
                            x.HeadAssignment.Should().NotBeNull();
                            x.InRelations.Should().HaveCount(1);
                            x.OutRelations.Should().HaveCount(1);
                        }
                    });
                }
                finally
                {
                    await RemoveTaskListAsync(mediator, t1.Id, t2.Id);
                }
            });
        }

        [Fact]
        public async Task GetList_withoutEmbed_should_ok()
        {
            await DoWorkAsync(async (client, sp) =>
            {
                var mediator = sp.GetRequiredService<IMediator>();
                var t1 = ((ApiResponseValue<WorkAssignmentDTO>)(await mediator.Send(CreateTaskCreationCommand(title: "Task #1"))).Value!).Payload!;
                var t2 = ((ApiResponseValue<WorkAssignmentDTO>)(await mediator.Send(CreateTaskCreationCommand(title: "Sub Task #1-1"))).Value!).Payload!;
                await mediator.Send(new AddSubWorkAssignmentCommand { WorkAssignmentId = t1.Id, SubWorkAssignmentId = t2.Id });

                try
                {
                    var data = await client.GetFromJsonAsync<ApiResponseValue<List<WorkAssignmentDTO>>>($"/{WorkAssignmentController.RootRoute}/list?embed=false", _jsonOptions);
                    data.Should().NotBeNull();
                    data.Payload.Should().HaveCountGreaterThanOrEqualTo(2);
                    data.Payload.ForEach(x =>
                    {
                        x.HeadAssignment.Should().BeNull();
                        x.SubAssignments.Should().BeNullOrEmpty();
                        x.InRelations.Should().BeNullOrEmpty();
                        x.OutRelations.Should().BeNullOrEmpty();
                    });
                }
                finally
                {
                    await RemoveTaskListAsync(mediator, t1.Id, t2.Id);
                }
            });
        }

        [Fact]
        public async Task GetList_onlyRoot_should_ok()
        {
            await DoWorkAsync(async (client, sp) =>
            {
                var mediator = sp.GetRequiredService<IMediator>();
                var t1 = ((ApiResponseValue<WorkAssignmentDTO>)(await mediator.Send(CreateTaskCreationCommand(title: "Task #1"))).Value!).Payload!;
                var t2 = ((ApiResponseValue<WorkAssignmentDTO>)(await mediator.Send(CreateTaskCreationCommand(title: "Sub Task #1-1"))).Value!).Payload!;
                await mediator.Send(new AddSubWorkAssignmentCommand { WorkAssignmentId = t1.Id, SubWorkAssignmentId = t2.Id });

                try
                {
                    var data = await client.GetFromJsonAsync<ApiResponseValue<List<WorkAssignmentDTO>>>($"/{WorkAssignmentController.RootRoute}/list?onlyRoot=true", _jsonOptions);
                    data.Should().NotBeNull();
                    data.Payload.Should().HaveCountGreaterThanOrEqualTo(1);
                    data.Payload.Should().OnlyContain(x => x.HeadAssignmentId == null);
                }
                finally
                {
                    await RemoveTaskListAsync(mediator, t1.Id, t2.Id);
                }
            });
        }

        #endregion

        #region Utils

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

        private async Task DoWorkAsync(Func<HttpClient, IServiceProvider, Task> work)
        {
            var client = await CreateClientWithJwt();
            using (var scope = _factory.Services.CreateScope())
            {
                await work(client, scope.ServiceProvider);
            }
        }

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
                taskIds.Add(((ApiResponseValue<WorkAssignmentDTO>)(await mediator.Send(CreateTaskCreationCommand(title: $"Task #{i + 1}"))).Value!).Payload!.Id);
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