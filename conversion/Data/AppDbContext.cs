package conversion.Data;

import Microsoft.EntityFrameworkCore;
import System.Threading.Tasks;

public class AppDbContext : DbContext {
    public DbSet<Student> Students { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {
    }

    public async Task AddStudentAsync(Student student) {
        await Students.AddAsync(student);
        await SaveChangesAsync();
    }

    public async Task<Student> GetStudentAsync(int id) {
        return await Students.FindAsync(id);
    }

    public async Task UpdateStudentAsync(Student student) {
        Students.Update(student);
        await SaveChangesAsync();
    }

    public async Task DeleteStudentAsync(int id) {
        var student = await Students.FindAsync(id);
        if (student != null) {
            Students.Remove(student);
            await SaveChangesAsync();
        }
    }
}

public class Student {
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
}