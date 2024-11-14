package conversion.Models;

import com.fasterxml.jackson.annotation.JsonProperty;
import com.fasterxml.jackson.databind.ObjectMapper;
import javax.validation.constraints.NotNull;
import java.time.format.DateTimeFormatter;
import java.time.format.DateTimeParseException;

public class Grade {
    private int Id;

    @NotNull
    @JsonProperty("studentId")
    public int StudentId;

    @NotNull
    @JsonProperty("courseId")
    public int CourseId;

    @NotNull
    @JsonProperty("value")
    public BigDecimal Value;

    @NotNull
    @JsonProperty("dateAssigned")
    public String DateAssigned;

    public int getId() {
        return Id;
    }

    public void setId(int id) {
        this.Id = id;
    }

    public static Grade fromJson(String json) {
        ObjectMapper objectMapper = new ObjectMapper();
        try {
            return objectMapper.readValue(json, Grade.class);
        } catch (Exception e) {
            throw new RuntimeException("Error parsing JSON", e);
        }
    }

    public void validate() throws IllegalArgumentException {
        if (StudentId <= 0) {
            throw new IllegalArgumentException("Invalid Student ID");
        }
        if (CourseId <= 0) {
            throw new IllegalArgumentException("Invalid Course ID");
        }
        if (Value.compareTo(BigDecimal.ZERO) < 0) {
            throw new IllegalArgumentException("Invalid Grade Value");
        }
        if (DateAssigned == null || DateAssigned.isEmpty()) {
            throw new IllegalArgumentException("Invalid Date Assigned");
        }
        try {
            DateTimeFormatter.ISO_DATE_TIME.parse(DateAssigned);
        } catch (DateTimeParseException e) {
            throw new IllegalArgumentException("Invalid Date Format for Date Assigned");
        }
        if (Id < 0) {
            throw new IllegalArgumentException("Invalid Id");
        }
    }
}