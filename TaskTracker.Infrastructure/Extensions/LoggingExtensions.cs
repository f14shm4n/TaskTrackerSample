using Microsoft.Extensions.Logging;

namespace TaskTracker.Infrastructure.Extensions
{
    internal static partial class LoggingExtensions
    {
        [LoggerMessage(Level = LogLevel.Error, Message = "Unable to commit transaction while deleting work assignment. WorkAssignmentId: '{Id}'")]
        public static partial void LogUnableToCommitTransactionOnDeleingWorkAssignment(this ILogger logger, int id, Exception ex);
    }
}
