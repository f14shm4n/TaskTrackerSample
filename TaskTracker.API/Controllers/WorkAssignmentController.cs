using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TaskTracker.API.Application;
using TaskTracker.API.Application.Commands;
using TaskTracker.API.Application.Dto;
using TaskTracker.API.Application.Queries;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TaskTracker.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route(RootRoute)]
    [Produces("application/json")]
    [ProducesResponseType(typeof(ApiResponseBase), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ApiResponseBase), (int)HttpStatusCode.InternalServerError)]
    public class WorkAssignmentController : ControllerBase
    {
        public const string RootRoute = "api/task";

        private readonly ILogger<WorkAssignmentController> _logger;
        private readonly IMediator _mediator;

        public WorkAssignmentController(ILogger<WorkAssignmentController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        /// <summary>
        /// Создает новую задачу.
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Объект содержащий созданную задачу.</returns>
        /// <response code="200">Возвращается если задача создана.</response>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseBase<WorkAssignmentDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ApiResponseBase<WorkAssignmentDTO>>> Create([FromBody] CreateWorkAssignmentCommand command)
        {
            return this.ToActionResultResult(await _mediator.Send(command));
        }

        /// <summary>
        /// Получает задачу по указанному идентификатору.
        /// </summary>
        /// <returns>Объект содержащий найденную задачу.</returns>
        /// <response code="200">Возвращается если задача найдена</response>
        /// <response code="404">Возвращается если задача не найдена.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponseBase<WorkAssignmentDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponseBase<WorkAssignmentDTO>), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ApiResponseBase<WorkAssignmentDTO>>> Get(GetWorkAssignmentByIdQuery query)
        {
            return this.ToActionResultResult(await _mediator.Send(query));
        }

        /// <summary>
        /// Получает коллекцию задач.
        /// </summary>
        /// <param name="query"></param>
        /// <returns>Объект содержащий список с задачами.</returns>
        [HttpGet("list")]
        [ProducesResponseType(typeof(ApiResponseBase<List<WorkAssignmentDTO>>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ApiResponseBase<List<WorkAssignmentDTO>>>> GetList(GetWorkAssignmentListQuery query)
        {
            return this.ToActionResultResult(await _mediator.Send(query));
        }

        /// <summary>
        /// Удаляет задачу по указанному идентификатору.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>    
        /// <response code="200">Возвращается если запрос выполнен без сбоев.</response>
        /// <response code="404">Возвращается если задача не найдена.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponseBase), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponseBase), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ApiResponseBase>> Delete(DeleteWorkAssignmentCommand command)
        {
            return this.ToActionResultResult(await _mediator.Send(command));
        }

        /// <summary>
        /// Устанавливает новый статус задачи.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        /// <response code="200">Возвращается в случае успешного выполнения.</response>
        /// <response code="404">Возвращается если задача не найдена.</response>
        [HttpPut("{id}/status/{status}")]
        [ProducesResponseType(typeof(ApiResponseBase), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponseBase), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ApiResponseBase>> UpdateStatus(UpdateWorkAssignmentStatusCommand command)
        {
            return this.ToActionResultResult(await _mediator.Send(command));
        }

        /// <summary>
        /// Устанавливает новый приоритет задачи.
        /// </summary>
        /// <param name="command"></param>
        /// <response code="200">Возвращается в случае успешного выполнения.</response>
        /// <response code="404">Возвращается если задача не найдена.</response>
        [HttpPut("{id}/priority/{priority}")]
        [ProducesResponseType(typeof(ApiResponseBase), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponseBase), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ApiResponseBase>> UpdatePriority(UpdateWorkAssignmentPriorityCommand command)
        {
            return this.ToActionResultResult(await _mediator.Send(command));
        }

        /// <summary>
        /// Устанавливает автора задачи.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        /// <response code="200">Возвращается в случае успешного выполнения.</response>
        /// <response code="404">Возвращается если задача не найдена.</response>
        [HttpPut("{id}/author/{author}")]
        [ProducesResponseType(typeof(ApiResponseBase), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponseBase), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ApiResponseBase>> UpdateAuthor(UpdateWorkAssignmentAuthorCommand command)
        {
            return this.ToActionResultResult(await _mediator.Send(command));
        }

        /// <summary>
        /// Устанавливает исполнителя задачи.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        /// <response code="200">Возвращается в случае успешного выполнения.</response>
        /// <response code="404">Возвращается если задача не найдена.</response>
        [HttpPut("{id}/worker/{worker?}")]
        [ProducesResponseType(typeof(ApiResponseBase), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponseBase), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ApiResponseBase>> UpdateWorker(UpdateWorkAssignmentWorkerCommand command)
        {
            return this.ToActionResultResult(await _mediator.Send(command));
        }

        /// <summary>
        /// Убирает исполнителя у указанной задачи.
        /// </summary>
        /// <param name="id">Идентификатор задачи.</param>
        /// <returns></returns>
        /// <response code="200">Возвращается в случае успешного выполнения.</response>
        /// <response code="404">Возвращается если задача не найдена.</response>
        [HttpPut("{id}/worker/unset")]
        [ProducesResponseType(typeof(ApiResponseBase), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponseBase), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ApiResponseBase>> UnsetWorker(int id)
        {
            return this.ToActionResultResult(await _mediator.Send(new UpdateWorkAssignmentWorkerCommand() { Id = id }));
        }

        /// <summary>
        /// Присоединяет указанную задачу как подзадачу к задаче.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        /// <response code="200">Возвращается в случае успешного выполнения.</response>
        /// <response code="400">Возвращается в случае, если данные запроса были некорректны. См. детали в объекте-результата запроса.</response>
        /// <response code="404">Возвращается если задача не найдена.</response>
        [HttpPut("{id}/nesting/{subTaskId}")]
        [ProducesResponseType(typeof(ApiResponseBase), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponseBase), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiResponseBase), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ApiResponseBase>> AddNesting(AddSubWorkAssignmentCommand command)
        {
            return this.ToActionResultResult(await _mediator.Send(command));
        }

        /// <summary>
        /// Отсоединяет подзадачу от другой задачу.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        /// <response code="200">Возвращается в случае успешного выполнения.</response>
        /// <response code="404">Возвращается если задача не найдена.</response>
        [HttpPut("{id}/unnesting")]
        [ProducesResponseType(typeof(ApiResponseBase), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponseBase), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ApiResponseBase>> RemoveNesting(ClearHeadWorkAssignmentCommand command)
        {
            return this.ToActionResultResult(await _mediator.Send(command));
        }

        /// <summary>
        /// Создает двунаправленную связь между задачами.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        /// <response code="200">Возвращается в случае успешного выполнения.</response>
        /// <response code="400">Возвращается в случае, если данные запроса были некорректны. См. детали в объекте-результата запроса.</response>
        /// <response code="404">Возвращается если задача не найдена.</response>
        [HttpPut("{sourceId}/relate/{targetId}/{relationType}")]
        [ProducesResponseType(typeof(ApiResponseBase), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponseBase), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiResponseBase), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ApiResponseBase>> Relate(AddWorkAssignmentRelationCommand command)
        {
            return this.ToActionResultResult(await _mediator.Send(command));
        }

        /// <summary>
        /// Удаляет двунаправленную связь между задачами.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        /// <response code="200">Возвращается в случае успешного выполнения.</response>
        /// <response code="400">Возвращается в случае, если данные запроса были некорректны. См. детали в объекте-результата запроса.</response>        
        [HttpPut("{sourceId}/unrelate/{targetId}/{relationType}")]
        [ProducesResponseType(typeof(ApiResponseBase), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponseBase), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ApiResponseBase>> Unrelate(RemoveWorkAssignmentRelationCommand command)
        {
            return this.ToActionResultResult(await _mediator.Send(command));
        }
    }
}
