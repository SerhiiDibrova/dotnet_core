package Logging;

using System;
using Serilog;

public interface ILoggerService
{
    void LogInformation(string message);
    void LogError(string message, Exception exception);
    void LogWarning(string message);
    void LogStudentCreationSuccess(string studentName);
    void LogValidationError(string errorMessage);
    void LogInternalServerError(Exception exception);
}

public class LoggerService : ILoggerService
{
    private readonly ILogger _logger;

    public LoggerService()
    {
        _logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();
    }

    public void LogInformation(string message)
    {
        _logger.Information(message);
    }

    public void LogError(string message, Exception exception)
    {
        _logger.Error(exception, message);
    }

    public void LogWarning(string message)
    {
        _logger.Warning(message);
    }

    public void LogStudentCreationSuccess(string studentName)
    {
        _logger.Information($"Student created successfully: {studentName}");
    }

    public void LogValidationError(string errorMessage)
    {
        _logger.Warning($"Validation error: {errorMessage}");
    }

    public void LogInternalServerError(Exception exception)
    {
        _logger.Error(exception, "An unexpected error occurred during the database save operation.");
    }
}