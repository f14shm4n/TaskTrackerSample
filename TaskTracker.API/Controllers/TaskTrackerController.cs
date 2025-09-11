using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TaskTracker.API.Application.Commands;
using TaskTracker.API.Application.Dto;
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
        /// <returns></returns>
        /// <response code="200">Возвращаетя если объект был успешно создан.</response>
        [HttpPost("create-task")]
        [ProducesResponseType(typeof(CreateWorkAssignmentCommandResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<CreateWorkAssignmentCommandResponse>> CreateTask([FromBody] CreateWorkAssignmentCommand command)
        {
            return await _mediator.Send(command);
        }

        /// <summary>
        /// Пытается найти и вернуть задачу по идентфикатору.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
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

        [HttpDelete("delete-task")]
        [ProducesResponseType(typeof(DeleteWorkAssignmentCommandResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<DeleteWorkAssignmentCommandResponse>> DeleteTask([FromQuery] DeleteWorkAssignmentCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpPut("update-task-status")]
        [ProducesResponseType(typeof(UpdateWorkAssignmentStatusCommandResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<UpdateWorkAssignmentStatusCommandResponse>> UpdateTaskStatus([FromBody] UpdateWorkAssignmentStatusCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpPut("update-task-priority")]
        [ProducesResponseType(typeof(UpdateWorkAssignmentPriorityCommandResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<UpdateWorkAssignmentPriorityCommandResponse>> UpdateTaskPriority([FromBody] UpdateWorkAssignmentPriorityCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpPut("update-task-author")]
        [ProducesResponseType(typeof(UpdateWorkAssignmentAuthorCommandResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<UpdateWorkAssignmentAuthorCommandResponse>> UpdateTaskAuthor([FromBody] UpdateWorkAssignmentAuthorCommand command)
        {
            return await _mediator.Send(command);
        }


        [HttpPut("update-task-worker")]
        [ProducesResponseType(typeof(UpdateWorkAssignmentWorkerCommandResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<UpdateWorkAssignmentWorkerCommandResponse>> UpdateTaskWorker([FromBody] UpdateWorkAssignmentWorkerCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpPut("add-sub-task")]
        [ProducesResponseType(typeof(AddSubWorkAssignmentCommandResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<AddSubWorkAssignmentCommandResponse>> AddSubTask([FromBody] AddSubWorkAssignmentCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpPut("clear-master-task")]
        [ProducesResponseType(typeof(ClearHeadWorkAssignmentCommandResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ClearHeadWorkAssignmentCommandResponse>> ClearMasterTask([FromBody] ClearHeadWorkAssignmentCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpPut("set-relation")]
        [ProducesResponseType(typeof(AddWorkAssignmentRelationCommandResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<AddWorkAssignmentRelationCommandResponse>> SetRelation([FromBody] AddWorkAssignmentRelationCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpPut("remove-relation")]
        [ProducesResponseType(typeof(RemoveWorkAssignmentRelationCommandResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<RemoveWorkAssignmentRelationCommandResponse>> RemoveRelation([FromBody] RemoveWorkAssignmentRelationCommand command)
        {
            return await _mediator.Send(command);
        }
    }
}
