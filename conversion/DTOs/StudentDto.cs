package conversion.DTOs;

import java.time.LocalDateTime;
import java.util.List;
import javax.persistence.EntityManager;
import javax.persistence.PersistenceContext;

public class StudentDto {
    private int id;
    private String firstName;
    private String lastName;
    private String email;
    private LocalDateTime enrollmentDate;

    public StudentDto(int id, String firstName, String lastName, String email, LocalDateTime enrollmentDate) {
        this.id = id;
        this.firstName = firstName;
        this.lastName = lastName;
        this.email = email;
        this.enrollmentDate = enrollmentDate;
    }

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public String getFirstName() {
        return firstName;
    }

    public void setFirstName(String firstName) {
        this.firstName = firstName;
    }

    public String getLastName() {
        return lastName;
    }

    public void setLastName(String lastName) {
        this.lastName = lastName;
    }

    public String getEmail() {
        return email;
    }

    public void setEmail(String email) {
        this.email = email;
    }

    public LocalDateTime getEnrollmentDate() {
        return enrollmentDate;
    }

    public void setEnrollmentDate(LocalDateTime enrollmentDate) {
        this.enrollmentDate = enrollmentDate;
    }

    @PersistenceContext
    private EntityManager _context;

    public List<StudentDto> retrieveStudents() {
        List<StudentDto> studentsList = _context.createQuery("SELECT new conversion.DTOs.StudentDto(s.id, s.firstName, s.lastName, s.email, s.enrollmentDate) FROM Student s", StudentDto.class).getResultList();
        return studentsList.isEmpty() ? List.of() : studentsList;
    }
}