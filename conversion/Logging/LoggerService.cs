namespace Conversion.Logging
{
    using Microsoft.Extensions.Logging;
    using Microsoft.AspNetCore.Mvc;

    public interface ILoggerService
    {
        void LogInformation(string message);
        void LogWarning(string message);
        void LogError(string message);
        void LogError(string message, Exception exception);
        IActionResult HandleDatabaseAccess(Action action);
    }

    public class LoggerService : ILoggerService
    {
        private readonly ILogger<LoggerService> _logger;

        public LoggerService(ILogger<LoggerService> logger)
        {
            _logger = logger;
        }

        public void LogInformation(string message)
        {
            _logger.LogInformation(message);
        }

        public void LogWarning(string message)
        {
            _logger.LogWarning(message);
        }

        public void LogError(string message)
        {
            _logger.LogError(message);
        }

        public void LogError(string message, Exception exception)
        {
            _logger.LogError(exception, message);
        }

        public IActionResult HandleDatabaseAccess(Action action)
        {
            try
            {
                action();
                return new OkResult();
            }
            catch (Exception ex)
            {
                LogError("An error occurred during database access.", ex);
                return new ObjectResult(new { error = "An internal server error occurred." }) { StatusCode = 500 };
            }
        }
    }
}