package Logging;

public interface ILoggerService {
    void LogInformation(String message);
    void LogWarning(String message);
    void LogError(String message);
}

package Controllers;

import Logging.ILoggerService;
import Services.StudentService;
import Services.ValidationException;

public class CreateStudentController {
    private final ILoggerService loggerService;
    private final StudentService studentService;

    public CreateStudentController(ILoggerService loggerService, StudentService studentService) {
        this.loggerService = loggerService;
        this.studentService = studentService;
    }

    public void createStudent(Student student) {
        try {
            studentService.validateStudent(student);
            studentService.saveStudent(student);
            loggerService.LogInformation("Successfully created student record for: " + student.getName());
        } catch (ValidationException e) {
            loggerService.LogWarning("Validation error for student: " + e.getMessage());
        } catch (Exception e) {
            loggerService.LogError("Error saving student record: " + e.getMessage());
        }
    }
}

package Services;

import Logging.ILoggerService;

public class StudentService {
    private final ILoggerService loggerService;

    public StudentService(ILoggerService loggerService) {
        this.loggerService = loggerService;
    }

    public void validateStudent(Student student) throws ValidationException {
        if (student.getName() == null || student.getName().isEmpty()) {
            loggerService.LogWarning("Student name is invalid.");
            throw new ValidationException("Student name cannot be empty.");
        }
        if (student.getAge() <= 0) {
            loggerService.LogWarning("Student age is invalid.");
            throw new ValidationException("Student age must be greater than zero.");
        }
    }

    public void saveStudent(Student student) throws Exception {
        try {
            // Database save logic
        } catch (Exception e) {
            loggerService.LogError("Exception during database save: " + e.getMessage());
            throw e;
        }
    }
}