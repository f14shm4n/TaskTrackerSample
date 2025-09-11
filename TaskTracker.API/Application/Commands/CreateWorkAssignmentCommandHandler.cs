using MediatR;
using TaskTracker.API.Application.Dto;
using TaskTracker.API.Application.Extensions;
using TaskTracker.Domain.Abstractions;
using TaskTracker.Domain.Aggregates.WorkAssignment;

namespace TaskTracker.API.Application.Commands
{
    public class CreateWorkAssignmentCommandHandler : IRequestHandler<CreateWorkAssignmentCommand, CreateWorkAssignmentCommandResponse>
    {
        private readonly ILogger<CreateWorkAssignmentCommandHandler> _logger;
        private readonly IWorkAssignmentRepository _taskRepository;

        public CreateWorkAssignmentCommandHandler(ILogger<CreateWorkAssignmentCommandHandler> logger, IWorkAssignmentRepository taskRepository)
        {
            _logger = logger;
            _taskRepository = taskRepository;
        }

        public async Task<CreateWorkAssignmentCommandResponse> Handle(CreateWorkAssignmentCommand request, CancellationToken cancellationToken)
        {
            CreateWorkAssignmentCommandResponse response;
            try
            {
                var entity = request.ToWorkAssignment();

                _taskRepository.Add(entity);
                await _taskRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

                response = new CreateWorkAssignmentCommandResponse(entity.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to add new work assignment to the data base.");
                response = new CreateWorkAssignmentCommandResponse(0);
            }
            return response;
        }
    }
}
