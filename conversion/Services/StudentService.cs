package conversion.Services;

import conversion.Models.Student;
import conversion.Models.StudentDto;
import conversion.Data.ApplicationDbContext;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Service;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import javax.validation.Valid;
import java.util.ArrayList;
import java.util.List;

@Service
public class StudentService {

    @Autowired
    private ApplicationDbContext _context;

    private static final Logger logger = LoggerFactory.getLogger(StudentService.class);

    public ResponseEntity<?> createStudent(@Valid StudentDto studentDto) {
        List<String> validationErrors = new ArrayList<>();

        if (studentDto.getName() == null || studentDto.getName().isEmpty()) {
            validationErrors.add("Name is required.");
        }

        if (studentDto.getEmail() == null || studentDto.getEmail().isEmpty()) {
            validationErrors.add("Email is required.");
        } else if (!isValidEmail(studentDto.getEmail())) {
            validationErrors.add("Invalid email format.");
        } else if (_context.Students.findByEmail(studentDto.getEmail()) != null) {
            validationErrors.add("Email must be unique.");
        }

        if (!validationErrors.isEmpty()) {
            logger.warn("Validation errors: {}", validationErrors);
            return ResponseEntity.badRequest().body(validationErrors);
        }

        Student student = new Student();
        student.setName(studentDto.getName());
        student.setEmail(studentDto.getEmail());

        try {
            _context.Students.add(student);
            _context.saveChanges();
            logger.info("Student created successfully: Name: {}, Email: {}", student.getName(), student.getEmail());
            return ResponseEntity.status(HttpStatus.CREATED).body(student);
        } catch (DataAccessException e) {
            logger.error("Database error occurred while creating student: {}", e.getMessage());
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body("An error occurred while creating the student.");
        } catch (Exception e) {
            logger.error("Unexpected error occurred while creating student: {}", e.getMessage());
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body("An error occurred while creating the student.");
        }
    }

    private boolean isValidEmail(String email) {
        String emailRegex = "^[A-Za-z0-9+_.-]+@(.+)$";
        return email.matches(emailRegex);
    }
}