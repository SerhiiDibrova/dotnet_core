package conversion.DTOs;

import com.fasterxml.jackson.annotation.JsonProperty;
import javax.validation.constraints.NotNull;
import javax.validation.constraints.PastOrPresent;
import javax.validation.constraints.DecimalMin;
import javax.validation.constraints.DecimalMax;
import java.util.Date;

public class GradeDto {
    private Long id;

    @NotNull
    @JsonProperty("studentId")
    private Long studentId;

    @NotNull
    @JsonProperty("courseId")
    private Long courseId;

    @NotNull
    @DecimalMin("0.0")
    @DecimalMax("100.0")
    @JsonProperty("value")
    private Double value;

    @NotNull
    @PastOrPresent
    @JsonProperty("dateAssigned")
    private Date dateAssigned;

    public GradeDto() {
    }

    public Long getId() {
        return id;
    }

    public void setId(Long id) {
        this.id = id;
    }

    public Long getStudentId() {
        return studentId;
    }

    public void setStudentId(Long studentId) {
        this.studentId = studentId;
    }

    public Long getCourseId() {
        return courseId;
    }

    public void setCourseId(Long courseId) {
        this.courseId = courseId;
    }

    public Double getValue() {
        return value;
    }

    public void setValue(Double value) {
        this.value = value;
    }

    public Date getDateAssigned() {
        return dateAssigned;
    }

    public void setDateAssigned(Date dateAssigned) {
        this.dateAssigned = dateAssigned;
    }
}