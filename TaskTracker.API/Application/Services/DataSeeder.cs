using Microsoft.EntityFrameworkCore;
using TaskTracker.Domain.Aggregates.WorkAssignment;
using TaskTracker.Infrastructure;

namespace TaskTracker.API.Application.Services
{
    public interface IDataSeeder
    {
        Task<ApiResponseBase> ClearAsync();
        Task<ApiResponseBase> FillAsync();
    }

    internal sealed class DataSeeder : IDataSeeder
    {
        private readonly ILogger<DataSeeder> _logger;
        private readonly TaskTrackerDbContext _context;

        public DataSeeder(ILogger<DataSeeder> logger, TaskTrackerDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<ApiResponseBase> FillAsync()
        {
            if (await HasAnyData())
            {
                return new ApiResponseBase("База данных уже содержит данные. Очистите базу данных и попробуйте снова.", System.Net.HttpStatusCode.BadRequest);
            }

            #region TestDataGen

            // 1. Новая срочная задача от менеджера
            var assignment1 = new WorkAssignment(
                "Срочный ремонт сервера в датацентре",
                WorkAssignmentStatus.New,
                WorkAssignmentPriority.High,
                "Иванов А.П.",
                null
            );

            // 2. Задача в работе для опытного сотрудника
            var assignment2 = new WorkAssignment(
                "Разработка нового API для мобильного приложения",
                WorkAssignmentStatus.InProgress,
                WorkAssignmentPriority.Medium,
                "Петрова С.И.",
                "Сидоров М.В."
            );

            // 3. Выполненная задача низкого приоритета
            var assignment3 = new WorkAssignment(
                "Обновление документации проекта",
                WorkAssignmentStatus.Done,
                WorkAssignmentPriority.Low,
                "Козлов Д.Н.",
                "Козлов Д.Н."
            );

            // 4. Новая задача среднего приоритета без исполнителя
            var assignment4 = new WorkAssignment(
                "Провести аудит безопасности системы",
                WorkAssignmentStatus.New,
                WorkAssignmentPriority.Medium,
                "Смирнов В.К.",
                null
            );

            // 5. Срочная задача в процессе выполнения
            var assignment5 = new WorkAssignment(
                "Устранение критической уязвимости в веб-приложении",
                WorkAssignmentStatus.InProgress,
                WorkAssignmentPriority.High,
                "Безопасность Отдел",
                "Васильев А.С."
            );

            // 6. Рутинная выполненная задача
            var assignment6 = new WorkAssignment(
                "Ежемесячное резервное копирование баз данных",
                WorkAssignmentStatus.Done,
                WorkAssignmentPriority.Low,
                "Система автоматизации",
                "Система автоматизации"
            );

            // 7. Новая задача планирования
            var assignment7 = new WorkAssignment(
                "Планирование миграции на новую версию ПО",
                WorkAssignmentStatus.New,
                WorkAssignmentPriority.Medium,
                "Технический директор",
                null
            );

            // 8. Задача в работе по улучшению производительности
            var assignment8 = new WorkAssignment(
                "Оптимизация запросов к базе данных",
                WorkAssignmentStatus.InProgress,
                WorkAssignmentPriority.High,
                "Отдел разработки",
                "Кузнецова Е.Л."
            );

            // 9. Выполненная задача обучения
            var assignment9 = new WorkAssignment(
                "Проведение обучения новых сотрудников",
                WorkAssignmentStatus.Done,
                WorkAssignmentPriority.Medium,
                "HR отдел",
                "Морозова Т.П."
            );

            // 10. Новая срочная задача от клиента
            var assignment10 = new WorkAssignment(
                "Срочный запрос от клиента Premium: доработка функционала",
                WorkAssignmentStatus.New,
                WorkAssignmentPriority.High,
                "Менеджер по работе с клиентами",
                null
            );

            #endregion

            await using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    await _context.WorkAssignments.AddRangeAsync(assignment1, assignment2, assignment3, assignment4, assignment5, assignment6, assignment7, assignment8, assignment9, assignment10);
                    await _context.SaveChangesAsync();

                    assignment5.SetHeadAssignment(assignment2.Id);
                    assignment6.SetHeadAssignment(assignment2.Id);
                    assignment9.SetHeadAssignment(assignment1.Id);
                    assignment10.SetHeadAssignment(assignment4.Id);

                    assignment7.AddInRelation(WorkAssignmentRelationType.RelativeTo, assignment2.Id);
                    assignment7.AddOutRelation(WorkAssignmentRelationType.RelativeTo, assignment2.Id);
                    assignment7.AddInRelation(WorkAssignmentRelationType.RelativeTo, assignment9.Id);
                    assignment7.AddOutRelation(WorkAssignmentRelationType.RelativeTo, assignment9.Id);

                    assignment10.AddInRelation(WorkAssignmentRelationType.RelativeTo, assignment1.Id);
                    assignment10.AddOutRelation(WorkAssignmentRelationType.RelativeTo, assignment1.Id);

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return new ApiResponseBase(true, System.Net.HttpStatusCode.OK);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to commit sql transaction while seeding data.");
                    await transaction.RollbackAsync();

                    return new ApiResponseBase("Failed to commit sql transaction while seeding data.", System.Net.HttpStatusCode.InternalServerError);
                }
            }
        }

        public async Task<ApiResponseBase> ClearAsync()
        {
            if (!(await HasAnyData()))
            {
                return new ApiResponseBase("База данных пуста.", System.Net.HttpStatusCode.BadRequest);
            }

            await using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    await _context.Database.ExecuteSqlRawAsync(
                        $@"DELETE FROM {TaskTrackerDbContext.WorkAssignmentRelationshipsTableName};
UPDATE {TaskTrackerDbContext.WorkAssignmentsTableName} SET {nameof(WorkAssignment.HeadAssignmentId)} = NULL;
DELETE FROM {TaskTrackerDbContext.WorkAssignmentsTableName};
DBCC CHECKIDENT ('{TaskTrackerDbContext.WorkAssignmentsTableName}', RESEED, 0)");
                    await transaction.CommitAsync();

                    return new ApiResponseBase(true);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to clear database.");
                    await transaction.RollbackAsync();

                    return new ApiResponseBase("Failed to clear database.", System.Net.HttpStatusCode.InternalServerError);
                }
            }
        }

        private Task<bool> HasAnyData() => _context.WorkAssignments.AnyAsync();
    }
}
