using TaskTracker.Domain.Aggregates.WorkAssignment;

namespace TaskTracker.API.Application.Extensions
{
    internal static partial class LoggingExtensions
    {
        [LoggerMessage(Level = LogLevel.Error, Message = "Unable to add nesting between WorkAssignmentId: '{Id}' and SubWorkAssignmentId: '{SubId}'")]
        public static partial void LogUnableToAddWorkAssignmentNesting(this ILogger logger, int id, int subId, Exception ex);

        [LoggerMessage(Level = LogLevel.Error, Message = "Unable to remove nesting from WorkAssignmentId: '{Id}'.")]
        public static partial void LogUnableToRemoveWorkAssignmentNesting(this ILogger logger, int id, Exception ex);

        [LoggerMessage(Level = LogLevel.Error, Message = "Unable to create relation between WorkAssignmentSourceId: '{SourceId}' and WorkAssignmentTargetId: '{TargetId}'")]
        public static partial void LogUnableToCreateWorkAssignmentRelation(this ILogger logger, int sourceId, int targetId, Exception ex);

        [LoggerMessage(Level = LogLevel.Error, Message = "Unable to remove relation between WorkAssignmentSourceId: '{SourceId}' and WorkAssignmentTargetId: '{TargetId}'")]
        public static partial void LogUnableToRemoveWorkAssignmentRelation(this ILogger logger, int sourceId, int targetId, Exception ex);

        [LoggerMessage(Level = LogLevel.Error, Message = "Unable to add new work assignment to the database.")]
        public static partial void LogUnableToCreateWorkAssignment(this ILogger logger, Exception ex);

        [LoggerMessage(Level = LogLevel.Error, Message = "Unable to delete work assignment. WorkAssignmentId: '{Id}'.")]
        public static partial void LogUnableToDeleteWorkAssignment(this ILogger logger, int id, Exception ex);

        [LoggerMessage(Level = LogLevel.Error, Message = "Unable to update work assignment author. WorkAssignmentId: '{Id}', NewAuthor: '{Author}'")]
        public static partial void LogUnableToUpdateWorkAssignmentAuthor(this ILogger logger, int id, string author, Exception ex);

        [LoggerMessage(Level = LogLevel.Error, Message = "Unable to update work assignment priority. WorkAssignmentId: '{Id}' and NewPriority: '{Priority}'")]
        public static partial void LogUnableToUpdateWorkAssignmentPriority(this ILogger logger, int id, WorkAssignmentPriority priority, Exception ex);

        [LoggerMessage(Level = LogLevel.Error, Message = "Unable to update work assignment status. WorkAssignmentId: '{Id}' and NewStatus: '{Status}'")]
        public static partial void LogUnableToUpdateWorkAssignmentStatus(this ILogger logger, int id, WorkAssignmentStatus status, Exception ex);

        [LoggerMessage(Level = LogLevel.Error, Message = "Unable to recall work assignment worker. WorkAssignmentId: '{Id}'")]
        public static partial void LogUnableToRecallWorkAssignmentWorker(this ILogger logger, int id, Exception ex);

        [LoggerMessage(Level = LogLevel.Error, Message = "Unable to update work assignment worker. WorkAssignmentId: '{Id}', NewWorker: '{Worker}'")]
        public static partial void LogUnableToUpdateWorkAssignmentWorker(this ILogger logger, int id, string worker, Exception ex);

        [LoggerMessage(Level = LogLevel.Error, Message = "Unable to get work assignment by id: '{Id}'")]
        public static partial void LogUnableToGetWorkAssignmentById(this ILogger logger, int id, Exception ex);

        [LoggerMessage(Level = LogLevel.Error, Message = "Unable to get work assignment list.")]
        public static partial void LogUnableToGetWorkAssignmentList(this ILogger logger, Exception ex);
    }
}
