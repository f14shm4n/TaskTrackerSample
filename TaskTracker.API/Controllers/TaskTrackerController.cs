using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TaskTracker.API.Application.Commands;
using TaskTracker.API.Application.Dto;
using TaskTracker.API.Application.Queries;

namespace TaskTracker.API.Controllers
{
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

        [HttpPost("create-task")]
        [ProducesResponseType(typeof(CreateTaskCommandResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<CreateTaskCommandResponse>> CreateTask([FromBody] CreateTaskCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpGet("get-task")]
        [ProducesResponseType(typeof(TaskDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<TaskDTO>> GetTask([FromQuery] GetTaskByIdQuery query)
        {
            var dto = await _mediator.Send(query);
            if (dto != null)
            {
                return dto;
            }
            return NotFound();
        }

        [HttpDelete("delete-task")]
        [ProducesResponseType(typeof(DeleteTaskCommandResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<DeleteTaskCommandResponse>> DeleteTask([FromQuery] DeleteTaskCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpPut("update-task-status")]
        [ProducesResponseType(typeof(UpdateTaskStatusCommandResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<UpdateTaskStatusCommandResponse>> UpdateTaskStatus([FromBody] UpdateTaskStatusCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpPut("update-task-priority")]
        [ProducesResponseType(typeof(UpdateTaskPriorityCommandResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<UpdateTaskPriorityCommandResponse>> UpdateTaskPriority([FromBody] UpdateTaskPriorityCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpPut("update-task-author")]
        [ProducesResponseType(typeof(UpdateTaskAuthorCommandResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<UpdateTaskAuthorCommandResponse>> UpdateTaskAuthor([FromBody] UpdateTaskAuthorCommand command)
        {
            return await _mediator.Send(command);
        }


        [HttpPut("update-task-worker")]
        [ProducesResponseType(typeof(UpdateTaskWorkerCommandResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<UpdateTaskWorkerCommandResponse>> UpdateTaskWorker([FromBody] UpdateTaskWorkerCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpPut("add-sub-task")]
        [ProducesResponseType(typeof(AddSubTaskCommandResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<AddSubTaskCommandResponse>> AddSubTask([FromBody] AddSubTaskCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpPut("clear-master-task")]
        [ProducesResponseType(typeof(ClearMasterTaskCommandResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ClearMasterTaskCommandResponse>> ClearMasterTask([FromBody] ClearMasterTaskCommand command)
        {
            return await _mediator.Send(command);
        }
    }
}
