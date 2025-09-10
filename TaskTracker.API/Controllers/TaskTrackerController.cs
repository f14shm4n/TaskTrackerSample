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
    }
}
