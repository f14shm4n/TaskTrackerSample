using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TaskTracker.API.Application;
using TaskTracker.API.Application.Commands;
using TaskTracker.API.Application.Dto;
using TaskTracker.API.Application.Queries;

namespace TaskTracker.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(ApiResponseBase), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ApiResponseBase), (int)HttpStatusCode.InternalServerError)]
    public class TaskTrackerController : ControllerBase
    {
        private readonly ILogger<TaskTrackerController> _logger;
        private readonly IMediator _mediator;

        public TaskTrackerController(ILogger<TaskTrackerController> logger, IMediator mediator)
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
        [HttpPost("create-task")]
        [ProducesResponseType(typeof(ApiResponseBase<WorkAssignmentDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ApiResponseBase<WorkAssignmentDTO>>> CreateTask([FromBody] CreateWorkAssignmentCommand command)
        {
            return this.ToActionResultResult(await _mediator.Send(command));
        }

        /// <summary>
        /// Получает задачу по указанному идентификатору.
        /// </summary>
        /// <param name="query"></param>
        /// <returns>Объект содержащий найденную задачу.</returns>
        /// <response code="200">Возвращается если задача найдена</response>
        /// <response code="404">Возвращается если задача не найдена.</response>
        [HttpGet("get-task")]
        [ProducesResponseType(typeof(ApiResponseBase<WorkAssignmentDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponseBase<WorkAssignmentDTO>), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ApiResponseBase<WorkAssignmentDTO>>> GetTask([FromQuery] GetWorkAssignmentByIdQuery query)
        {
            return this.ToActionResultResult(await _mediator.Send(query));
        }

        /// <summary>
        /// Получает коллекцию задач.
        /// </summary>
        /// <param name="query"></param>
        /// <returns>Объект содержащий список с задачами.</returns>
        [HttpGet("get-tasks")]
        [ProducesResponseType(typeof(ApiResponseBase<List<WorkAssignmentDTO>>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ApiResponseBase<List<WorkAssignmentDTO>>>> GetTasks([FromQuery] GetWorkAssignmentListQuery query)
        {
            return this.ToActionResultResult(await _mediator.Send(query));
        }

        /// <summary>
        /// Удаляет задачу по указанному идентификатору.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>    
        /// <response code="200">Возвращается если запрос выполнен без сбоев.</response>
        [HttpDelete("delete-task")]
        [ProducesResponseType(typeof(ApiResponseBase), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ApiResponseBase>> DeleteTask([FromQuery] DeleteWorkAssignmentCommand command)
        {
            return this.ToActionResultResult(await _mediator.Send(command));
        }

        /// <summary>
        /// Устанавливает новый статус задачи.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        /// <response code="200">Возвращается в случае успешного выполнения.</response>
        /// <response code="400">Возвращается в случае, если данные запроса были некорректны. См. детали в объекте-результата запроса.</response>
        [HttpPut("update-task-status")]
        [ProducesResponseType(typeof(ApiResponseBase), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ApiResponseBase>> UpdateTaskStatus([FromBody] UpdateWorkAssignmentStatusCommand command)
        {
            return this.ToActionResultResult(await _mediator.Send(command));
        }

        /// <summary>
        /// Устанавливает новый приоритет задачи.
        /// </summary>
        /// <param name="command"></param>
        /// <response code="200">Возвращается в случае успешного выполнения.</response>
        /// <response code="400">Возвращается в случае, если данные запроса были некорректны. См. детали в объекте-результата запроса.</response>
        [HttpPut("update-task-priority")]
        [ProducesResponseType(typeof(ApiResponseBase), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ApiResponseBase>> UpdateTaskPriority([FromBody] UpdateWorkAssignmentPriorityCommand command)
        {
            return this.ToActionResultResult(await _mediator.Send(command));
        }

        /// <summary>
        /// Устанавливает автора задачи.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        /// <response code="200">Возвращается в случае успешного выполнения.</response>
        /// <response code="400">Возвращается в случае, если данные запроса были некорректны. См. детали в объекте-результата запроса.</response>
        [HttpPut("update-task-author")]
        [ProducesResponseType(typeof(ApiResponseBase), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ApiResponseBase>> UpdateTaskAuthor([FromBody] UpdateWorkAssignmentAuthorCommand command)
        {
            return this.ToActionResultResult(await _mediator.Send(command));
        }

        /// <summary>
        /// Устанавливает исполнителя задачи.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        /// <response code="200">Возвращается в случае успешного выполнения.</response>
        /// <response code="400">Возвращается в случае, если данные запроса были некорректны. См. детали в объекте-результата запроса.</response>
        [HttpPut("update-task-worker")]
        [ProducesResponseType(typeof(ApiResponseBase), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ApiResponseBase>> UpdateTaskWorker([FromBody] UpdateWorkAssignmentWorkerCommand command)
        {
            return this.ToActionResultResult(await _mediator.Send(command));
        }

        /// <summary>
        /// Присоединяет указанную задачу как подзадачу к задаче.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        /// <response code="200">Возвращается в случае успешного выполнения.</response>
        /// <response code="400">Возвращается в случае, если данные запроса были некорректны. См. детали в объекте-результата запроса.</response>
        [HttpPut("add-sub-task")]
        [ProducesResponseType(typeof(ApiResponseBase), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponseBase), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ApiResponseBase>> AddSubTask([FromBody] AddSubWorkAssignmentCommand command)
        {
            return this.ToActionResultResult(await _mediator.Send(command));
        }

        /// <summary>
        /// Отсоединяет подзадачу от другой задачу.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        /// <response code="200">Возвращается в случае успешного выполнения.</response>
        /// <response code="400">Возвращается в случае, если данные запроса были некорректны. См. детали в объекте-результата запроса.</response>
        [HttpPut("clear-master-task")]
        [ProducesResponseType(typeof(ApiResponseBase), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ApiResponseBase>> ClearMasterTask([FromBody] ClearHeadWorkAssignmentCommand command)
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
        [HttpPut("set-relation")]
        [ProducesResponseType(typeof(ApiResponseBase), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponseBase), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ApiResponseBase>> SetRelation([FromBody] AddWorkAssignmentRelationCommand command)
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
        [HttpPut("remove-relation")]
        [ProducesResponseType(typeof(ApiResponseBase), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponseBase), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ApiResponseBase>> RemoveRelation([FromBody] RemoveWorkAssignmentRelationCommand command)
        {
            return this.ToActionResultResult(await _mediator.Send(command));
        }
    }
}
