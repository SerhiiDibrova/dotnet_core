package conversion.Data;

import Microsoft.EntityFrameworkCore;
import System.Collections.Generic;
import System.Threading.Tasks;

public class AppDbContext : DbContext
{
    public DbSet<Student> Students { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }

    public async Task<List<Student>> GetStudentsAsync()
    {
        return await Students.ToListAsync();
    }

    public async Task AddStudentAsync(Student student)
    {
        await Students.AddAsync(student);
        await SaveChangesAsync();
    }
}

public class Student
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
}