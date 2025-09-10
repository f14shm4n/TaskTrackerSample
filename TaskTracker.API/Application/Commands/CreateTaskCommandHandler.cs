using MediatR;
using TaskTracker.API.Application.Dto;
using TaskTracker.API.Application.Extensions;
using TaskTracker.Domain.Abstractions;
using TaskTracker.Domain.Aggregates.Tasks;

namespace TaskTracker.API.Application.Commands
{
    public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, CreateTaskCommandResponse>
    {
        private readonly ILogger<CreateTaskCommandHandler> _logger;
        private readonly ITaskRepository _taskRepository;

        public CreateTaskCommandHandler(ILogger<CreateTaskCommandHandler> logger, ITaskRepository taskRepository)
        {
            _logger = logger;
            _taskRepository = taskRepository;
        }

        public async Task<CreateTaskCommandResponse> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            CreateTaskCommandResponse response;
            try
            {
                var entity = request.ToTaskEntity();

                _taskRepository.Add(entity);
                await _taskRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

                response = new CreateTaskCommandResponse(entity.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to add new task to the data base.");
                response = new CreateTaskCommandResponse(0);
            }
            return response;
        }
    }
}
