package Data;

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Collections.Generic;

public class AppDbContext : DbContext
{
    public DbSet<Student> Students { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>().ToTable("Students");
    }

    public async Task AddStudentAsync(Student student)
    {
        if (student == null || !ValidateStudent(student))
        {
            throw new ArgumentException("Invalid student data.");
        }

        try
        {
            await Students.AddAsync(student);
            await SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            throw new Exception("An error occurred while saving the student.", ex);
        }
    }

    private bool ValidateStudent(Student student)
    {
        var validationContext = new ValidationContext(student);
        var validationResults = new List<ValidationResult>();
        return Validator.TryValidateObject(student, validationContext, validationResults, true);
    }
}

public class Student
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [Range(1, 120)]
    public int Age { get; set; }
}