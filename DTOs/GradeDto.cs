package DTOs;

import javax.validation.constraints.NotNull;

public class GradeDto {
    @NotNull
    private int id;

    @NotNull
    private String subject;

    @NotNull
    private double score;

    @NotNull
    private String term;

    public GradeDto(int id, String subject, double score, String term) {
        this.id = id;
        this.subject = subject;
        this.score = score;
        this.term = term;
    }

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public String getSubject() {
        return subject;
    }

    public void setSubject(String subject) {
        this.subject = subject;
    }

    public double getScore() {
        return score;
    }

    public void setScore(double score) {
        this.score = score;
    }

    public String getTerm() {
        return term;
    }

    public void setTerm(String term) {
        this.term = term;
    }
}

package DTOs;

import javax.validation.Valid;
import java.util.List;

public class StudentResponseDto {
    @Valid
    private StudentDto student;

    @Valid
    private List<GradeDto> grades;

    public StudentResponseDto(StudentDto student, List<GradeDto> grades) {
        this.student = student;
        this.grades = grades;
    }

    public StudentDto getStudent() {
        return student;
    }

    public void setStudent(StudentDto student) {
        this.student = student;
    }

    public List<GradeDto> getGrades() {
        return grades;
    }

    public void setGrades(List<GradeDto> grades) {
        this.grades = grades;
    }
}

package Controllers;

import DTOs.StudentResponseDto;
import DTOs.StudentDto;
import DTOs.GradeDto;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import javax.validation.Valid;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;

@RestController
@RequestMapping("/api/students")
public class StudentController {

    @GetMapping("/{id}/grades")
    public StudentResponseDto getStudentGrades(@PathVariable int id) {
        StudentDto student = new StudentDto(id, "John Doe", "john.doe@example.com", new Date(946684800000L));
        List<GradeDto> grades = new ArrayList<>();
        grades.add(new GradeDto(1, "Mathematics", 95.0, "Term 1"));
        grades.add(new GradeDto(2, "Science", 88.0, "Term 1"));
        return new StudentResponseDto(student, grades);
    }
}