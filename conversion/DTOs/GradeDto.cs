package conversion.DTOs;

import java.math.BigDecimal;
import java.time.LocalDateTime;
import javax.validation.constraints.NotNull;
import javax.validation.constraints.DecimalMin;
import javax.validation.constraints.PastOrPresent;

public class GradeDto {
    @NotNull
    public Integer Id;

    @NotNull
    public int StudentId;

    @NotNull
    public int CourseId;

    @NotNull
    @DecimalMin(value = "0.0", inclusive = false)
    public BigDecimal Value;

    @NotNull
    @PastOrPresent
    public LocalDateTime DateAssigned;
}