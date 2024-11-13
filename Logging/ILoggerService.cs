package Logging;

public interface ILoggerService {
    void LogInfo(String message);
    void LogError(String message);
    void LogStudentCreated(String studentName);
    void LogValidationError(String errorMessage);
    void LogDatabaseException(Exception ex);
}

package Logging;

import java.util.logging.Level;
import java.util.logging.Logger;

public class LoggerService implements ILoggerService {
    private static final Logger logger = Logger.getLogger(LoggerService.class.getName());

    public void LogInfo(String message) {
        logger.log(Level.INFO, message);
    }

    public void LogError(String message) {
        logger.log(Level.SEVERE, message);
    }

    public void LogStudentCreated(String studentName) {
        LogInfo("Successfully created student record for: " + studentName);
    }

    public void LogValidationError(String errorMessage) {
        LogError("Validation error: " + errorMessage);
    }

    public void LogDatabaseException(Exception ex) {
        LogError("Database operation failed: " + ex.getMessage());
    }
}