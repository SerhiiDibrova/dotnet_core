namespace Conversion.Logging
{
    using System;

    public interface ILoggerService
    {
        void LogError(string message, Exception ex);
        void LogInfo(string message);
        (int StatusCode, object Response) HandleDatabaseAccessException(Exception ex);
    }

    public class LoggerService : ILoggerService
    {
        public void LogError(string message, Exception ex)
        {
            // Actual logging logic for errors
        }

        public void LogInfo(string message)
        {
            // Actual logging logic for information
        }

        public (int StatusCode, object Response) HandleDatabaseAccessException(Exception ex)
        {
            return (500, new { ErrorMessage = "An error occurred while accessing the database.", ExceptionMessage = ex.Message });
        }
    }
}