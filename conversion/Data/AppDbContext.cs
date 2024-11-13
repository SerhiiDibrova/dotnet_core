package conversion.Data;

import Microsoft.EntityFrameworkCore;
import Microsoft.EntityFrameworkCore.Infrastructure;
import Microsoft.EntityFrameworkCore.Storage;
import System;
import System.Linq;
import System.Threading.Tasks;
import Microsoft.Extensions.Logging;

public class AppDbContext : DbContext
{
    private readonly ILogger<AppDbContext> _logger;

    public DbSet<Grade> Grades { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Course> Courses { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options, ILogger<AppDbContext> logger) : base(options)
    {
        _logger = logger;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Grade>().ToTable("Grades");
        modelBuilder.Entity<Student>().ToTable("Students");
        modelBuilder.Entity<Course>().ToTable("Courses");
    }

    public async Task<IActionResult> SaveChangesAsync()
    {
        try
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is Student student)
                {
                    var validationResult = ValidateStudent(student);
                    if (validationResult != null) return validationResult;
                }
                else if (entry.Entity is Course course)
                {
                    var validationResult = ValidateCourse(course);
                    if (validationResult != null) return validationResult;
                }
                else if (entry.Entity is Grade grade)
                {
                    var validationResult = ValidateGrade(grade);
                    if (validationResult != null) return validationResult;
                }
            }
            await base.SaveChangesAsync();
            return new OkResult();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "500 Internal Server Error: An unexpected error occurred while saving changes.");
            return new StatusCodeResult(500);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "500 Internal Server Error: An unexpected error occurred.");
            return new StatusCodeResult(500);
        }
    }

    public IActionResult ValidateStudent(Student student)
    {
        if (student == null || string.IsNullOrEmpty(student.Name) || student.Id <= 0)
        {
            _logger.LogWarning("400 Bad Request: Invalid student data.");
            return new BadRequestObjectResult("400 Bad Request: Invalid student data.");
        }
        return null;
    }

    public IActionResult ValidateCourse(Course course)
    {
        if (course == null || string.IsNullOrEmpty(course.Title) || course.Id <= 0)
        {
            _logger.LogWarning("400 Bad Request: Invalid course data.");
            return new BadRequestObjectResult("400 Bad Request: Invalid course data.");
        }
        return null;
    }

    public IActionResult ValidateGrade(Grade grade)
    {
        if (grade == null || grade.Score < 0 || grade.StudentId <= 0 || grade.CourseId <= 0)
        {
            _logger.LogWarning("400 Bad Request: Invalid grade data.");
            return new BadRequestObjectResult("400 Bad Request: Invalid grade data.");
        }
        if (!Students.Any(s => s.Id == grade.StudentId))
        {
            _logger.LogWarning("400 Bad Request: Non-existent student ID.");
            return new BadRequestObjectResult("400 Bad Request: Non-existent student ID.");
        }
        if (!Courses.Any(c => c.Id == grade.CourseId))
        {
            _logger.LogWarning("400 Bad Request: Non-existent course ID.");
            return new BadRequestObjectResult("400 Bad Request: Non-existent course ID.");
        }
        return null;
    }
}