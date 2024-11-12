package conversion.Data;

import Microsoft.EntityFrameworkCore;
import System.Collections.Generic;
import System.ComponentModel.DataAnnotations;
import Microsoft.Extensions.Logging;

public class AppDbContext : DbContext
{
    private readonly ILogger<AppDbContext> _logger;

    public DbSet<Student> Students { get; set; }
    public DbSet<Grade> Grades { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options, ILogger<AppDbContext> logger) : base(options)
    {
        _logger = logger;
    }

    public Student GetStudentWithGrades(int studentId)
    {
        try
        {
            return Students.Include(s => s.Grades).FirstOrDefault(s => s.Id == studentId);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving the student with grades.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred.");
            throw;
        }
    }
}

public class Student
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    public ICollection<Grade> Grades { get; set; }
}

public class Grade
{
    public int Id { get; set; }

    [Required]
    public string Subject { get; set; }

    [Range(0, 100)]
    public int Score { get; set; }

    public int StudentId { get; set; }
    public Student Student { get; set; }
}