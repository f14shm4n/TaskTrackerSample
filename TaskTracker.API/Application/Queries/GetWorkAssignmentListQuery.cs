using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskTracker.API.Application.Dto;

namespace TaskTracker.API.Application.Queries
{
    public class GetWorkAssignmentListQuery : IRequest<ApiResponseBase<List<WorkAssignmentDTO>>>
    {
        /// <summary>
        /// Определяет, следует ли извлекать связанные данные задачи, такие как подзадачи, связи с другими задачами и тд.
        /// </summary>
        [FromQuery(Name = "embed")]
        public bool WithEmbedData { get; set; }
        /// <summary>
        /// Определяет, следует ли возвращать только задачи корневого уровня.
        /// </summary>
        [FromQuery(Name = "onlyRoot")]
        public bool OnlyHeadTasks { get; set; }
        /// <summary>
        /// Задаёт идентификатор задачи как курсор, т.е. запрос вернет коллекцию задач которые следуют курсором.
        /// </summary>
        [FromQuery(Name = "cursor")]
        public int Cursor { get; set; } = 0;
        /// <summary>
        /// Устанавливает максимальное кол-во задач в выборке. По умолчанию: 10.
        /// </summary>
        [FromQuery(Name = "limit")]
        public int Limit { get; set; } = 10;
    }
}
