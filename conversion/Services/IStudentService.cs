package conversion.Services;

import java.util.List;
import java.util.concurrent.CompletableFuture;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

@Service
public class StudentService implements IStudentService {
    
    private final AppDbContext _context;

    @Autowired
    public StudentService(AppDbContext context) {
        this._context = context;
    }

    @Override
    public CompletableFuture<List<Student>> GetAllStudentsAsync() {
        return CompletableFuture.supplyAsync(() -> {
            try {
                return _context.Students.findAll();
            } catch (Exception e) {
                throw new RuntimeException("Error retrieving students", e);
            }
        });
    }
}