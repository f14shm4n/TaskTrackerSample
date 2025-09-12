using MediatR;
using TaskTracker.API.Application.Dto;
using TaskTracker.API.Application.Extensions;
using TaskTracker.Domain.Aggregates.WorkAssignment;

namespace TaskTracker.API.Application.Commands
{
    public class CreateWorkAssignmentCommandHandler : IRequestHandler<CreateWorkAssignmentCommand, ApiResponseBase<WorkAssignmentDTO>>
    {
        private readonly ILogger<CreateWorkAssignmentCommandHandler> _logger;
        private readonly IWorkAssignmentRepository _workRepository;

        public CreateWorkAssignmentCommandHandler(ILogger<CreateWorkAssignmentCommandHandler> logger, IWorkAssignmentRepository taskRepository)
        {
            _logger = logger;
            _workRepository = taskRepository;
        }

        public async Task<ApiResponseBase<WorkAssignmentDTO>> Handle(CreateWorkAssignmentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = request.ToWorkAssignment();
                _workRepository.Add(entity);
                await _workRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
                return new ApiResponseBase<WorkAssignmentDTO>(entity.ToWorkAssignmentDto());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to add new work assignment to the data base.");
            }
            return new ApiResponseBase<WorkAssignmentDTO>(false, System.Net.HttpStatusCode.InternalServerError);
        }
    }
}
