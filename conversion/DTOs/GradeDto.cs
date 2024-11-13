package conversion.DTOs;

import javax.validation.constraints.NotNull;
import javax.validation.constraints.Pattern;
import javax.validation.constraints.Min;
import javax.validation.constraints.Max;
import com.fasterxml.jackson.annotation.JsonProperty;

public class GradeDto {
    @JsonProperty("id")
    private Long id;

    @NotNull
    @JsonProperty("studentId")
    private Long studentId;

    @NotNull
    @JsonProperty("courseId")
    private Long courseId;

    @NotNull
    @Min(0)
    @Max(100)
    @JsonProperty("value")
    private Integer value;

    @NotNull
    @Pattern(regexp = "^(\\d{4}-\\d{2}-\\d{2}T\\d{2}:\\d{2}:\\d{2}(\\.\\d{1,3})?Z|\\d{4}-\\d{2}-\\d{2}T\\d{2}:\\d{2}:\\d{2}(\\.\\d{1,3})?([+-]\\d{2}:\\d{2}|[+-]\\d{2}\\d{2})|\\d{4}-\\d{2}-\\d{2})$", message = "Date must be in ISO 8601 format")
    @JsonProperty("dateAssigned")
    private String dateAssigned;

    public GradeDto(Long id, Long studentId, Long courseId, Integer value, String dateAssigned) {
        this.id = id;
        this.studentId = studentId;
        this.courseId = courseId;
        this.value = value;
        this.dateAssigned = dateAssigned;
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

    public Integer getValue() {
        return value;
    }

    public void setValue(Integer value) {
        this.value = value;
    }

    public String getDateAssigned() {
        return dateAssigned;
    }

    public void setDateAssigned(String dateAssigned) {
        this.dateAssigned = dateAssigned;
    }
}