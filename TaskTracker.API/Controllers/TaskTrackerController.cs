﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TaskTracker.API.Application.Commands;
using TaskTracker.API.Application.Queries;

namespace TaskTracker.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
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
        [HttpPost("create-task")]
        [ProducesResponseType(typeof(CreateWorkAssignmentCommandResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<CreateWorkAssignmentCommandResponse>> CreateTask([FromBody] CreateWorkAssignmentCommand command)
        {
            return await _mediator.Send(command);
        }

        /// <summary>
        /// Получает задачу по указанному идентификатору.
        /// </summary>
        /// <param name="query"></param>
        /// <returns>Объект содержащий найденную задачу.</returns>
        /// <response code="200">Возвращается если задача найдена</response>
        /// <response code="404">Возвращается если задача не найдена.</response>
        [HttpGet("get-task")]
        [ProducesResponseType(typeof(GetWorkAssignmentByIdQueryResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<GetWorkAssignmentByIdQueryResponse>> GetTask([FromQuery] GetWorkAssignmentByIdQuery query)
        {
            var rsp = await _mediator.Send(query);
            if (rsp.TaskInfo is null)
            {
                return NotFound();
            }
            return rsp;
        }

        /// <summary>
        /// Получает коллекцию задач.
        /// </summary>
        /// <param name="query"></param>
        /// <returns>Объект содержащий список с задачами.</returns>
        [HttpGet("get-tasks")]
        [ProducesResponseType(typeof(GetWorkAssignmentByIdQueryResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<GetWorkAssignmentListQueryResponse>> GetTasks([FromQuery] GetWorkAssignmentListQuery query)
        {
            return await _mediator.Send(query);
        }

        /// <summary>
        /// Удаляет задачу по указанному идентификатору.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>    
        [HttpDelete("delete-task")]
        [ProducesResponseType(typeof(DeleteWorkAssignmentCommandResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<DeleteWorkAssignmentCommandResponse>> DeleteTask([FromQuery] DeleteWorkAssignmentCommand command)
        {
            return await _mediator.Send(command);
        }

        /// <summary>
        /// Устанавливает новый статус задачи.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("update-task-status")]
        [ProducesResponseType(typeof(UpdateWorkAssignmentStatusCommandResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<UpdateWorkAssignmentStatusCommandResponse>> UpdateTaskStatus([FromBody] UpdateWorkAssignmentStatusCommand command)
        {
            return await _mediator.Send(command);
        }

        /// <summary>
        /// Устанавливает новый приоритет задачи.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("update-task-priority")]
        [ProducesResponseType(typeof(UpdateWorkAssignmentPriorityCommandResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<UpdateWorkAssignmentPriorityCommandResponse>> UpdateTaskPriority([FromBody] UpdateWorkAssignmentPriorityCommand command)
        {
            return await _mediator.Send(command);
        }

        /// <summary>
        /// Устанавливает автора задачи.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("update-task-author")]
        [ProducesResponseType(typeof(UpdateWorkAssignmentAuthorCommandResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<UpdateWorkAssignmentAuthorCommandResponse>> UpdateTaskAuthor([FromBody] UpdateWorkAssignmentAuthorCommand command)
        {
            return await _mediator.Send(command);
        }

        /// <summary>
        /// Устанавливает исполнителя задачи.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("update-task-worker")]
        [ProducesResponseType(typeof(UpdateWorkAssignmentWorkerCommandResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<UpdateWorkAssignmentWorkerCommandResponse>> UpdateTaskWorker([FromBody] UpdateWorkAssignmentWorkerCommand command)
        {
            return await _mediator.Send(command);
        }

        /// <summary>
        /// Присоединяет указанную задачу как подзадачу к задаче.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("add-sub-task")]
        [ProducesResponseType(typeof(AddSubWorkAssignmentCommandResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<AddSubWorkAssignmentCommandResponse>> AddSubTask([FromBody] AddSubWorkAssignmentCommand command)
        {
            return await _mediator.Send(command);
        }

        /// <summary>
        /// Отсоединяет подзадачу от другой задачу.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("clear-master-task")]
        [ProducesResponseType(typeof(ClearHeadWorkAssignmentCommandResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ClearHeadWorkAssignmentCommandResponse>> ClearMasterTask([FromBody] ClearHeadWorkAssignmentCommand command)
        {
            return await _mediator.Send(command);
        }

        /// <summary>
        /// Создает двунаправленную связь между задачами.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("set-relation")]
        [ProducesResponseType(typeof(AddWorkAssignmentRelationCommandResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<AddWorkAssignmentRelationCommandResponse>> SetRelation([FromBody] AddWorkAssignmentRelationCommand command)
        {
            return await _mediator.Send(command);
        }

        /// <summary>
        /// Удаляет двунаправленную связь между задачами.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("remove-relation")]
        [ProducesResponseType(typeof(RemoveWorkAssignmentRelationCommandResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<RemoveWorkAssignmentRelationCommandResponse>> RemoveRelation([FromBody] RemoveWorkAssignmentRelationCommand command)
        {
            return await _mediator.Send(command);
        }
    }
}
