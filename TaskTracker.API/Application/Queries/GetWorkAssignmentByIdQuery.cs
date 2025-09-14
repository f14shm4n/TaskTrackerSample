using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskTracker.API.Application.Dto;

namespace TaskTracker.API.Application.Queries
{
    public class GetWorkAssignmentByIdQuery : IRequest<ApiResponseBase<WorkAssignmentDTO>>
    {
        /// <summary>
        /// Идентификатор задачи, которую нужно получить.
        /// </summary>
        [FromRoute(Name = "id")]
        public int Id { get; set; }
        /// <summary>
        /// Определяет, следует ли извлекать связанные данные задачи, такие как подзадачи, связи с другими задачами и тд.
        /// </summary>
        [FromQuery(Name = "embed")]
        public bool WithEmbedData { get; set; }
    }
}
