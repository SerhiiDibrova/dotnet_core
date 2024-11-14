package conversion.Services;

import java.util.List;
import java.util.stream.Collectors;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.http.HttpStatus;
import org.springframework.stereotype.Service;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.transaction.annotation.Transactional;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.stereotype.Repository;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.repository.query.Param;

@Service
public class StudentService {

    private static final Logger logger = LoggerFactory.getLogger(StudentService.class);

    @Autowired
    private StudentRepository _context;

    @Transactional(readOnly = true)
    public ResponseEntity<List<StudentDto>> getAllStudentsAsync() {
        try {
            List<Student> studentsList = _context.findAll();
            List<StudentDto> studentDtos = studentsList.stream()
                .map(student -> new StudentDto(student.getId(), student.getName(), student.getEmail()))
                .collect(Collectors.toList());
            return ResponseEntity.ok(studentDtos);
        } catch (Exception e) {
            logger.error("Error retrieving student records", e);
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(null);
        }
    }
}

@Repository
interface StudentRepository extends JpaRepository<Student, Long> {
}

class Student {
    private Long id;
    private String name;
    private String email;

    // Getters and Setters
}

class StudentDto {
    private Long id;
    private String name;
    private String email;

    public StudentDto(Long id, String name, String email) {
        this.id = id;
        this.name = name;
        this.email = email;
    }

    // Getters and Setters
}