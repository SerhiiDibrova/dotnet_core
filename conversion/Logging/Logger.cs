package conversion.Logging;

import java.util.logging.Level;
import java.util.logging.Logger;

public class LoggerUtil {
    private static final Logger logger = Logger.getLogger(LoggerUtil.class.getName());

    public static void logStudentCreation(String studentId) {
        logger.log(Level.INFO, "Successfully created student record with ID: " + studentId);
    }

    public static void logValidationError(String errorMessage) {
        logger.log(Level.WARNING, "Validation error: " + errorMessage);
    }

    public static void logDatabaseException(String message, Exception e) {
        logger.log(Level.SEVERE, "Database operation failed: " + message, e);
    }
}