package conversion.Interfaces;

import java.util.concurrent.CompletableFuture;
import javax.validation.Valid;
import javax.validation.constraints.NotNull;

public interface IStudentService {
    CompletableFuture<StudentDto> CreateStudentAsync(@Valid @NotNull StudentDto studentDto);
}