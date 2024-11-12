using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public interface IGradeService
{
    Task<Grade> AddGradeAsync(Grade grade);
}

public class Grade
{
    public int Id { get; set; }
    public string StudentId { get; set; }
    public string Subject { get; set; }
    public double Value { get; set; }
}

public class GradeContext : DbContext
{
    public DbSet<Grade> Grades { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("GradesDb");
    }
}

public class GradeService : IGradeService
{
    private readonly GradeContext _context;
    private readonly ILogger<GradeService> _logger;

    public GradeService(GradeContext context, ILogger<GradeService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Grade> AddGradeAsync(Grade grade)
    {
        try
        {
            ValidateGrade(grade);
            _context.Grades.Add(grade);
            await _context.SaveChangesAsync();
            return grade;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database update error while adding grade for StudentId: {StudentId}", grade.StudentId);
            throw new Exception("Database update error while adding the grade.", ex);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Validation error for StudentId: {StudentId}", grade.StudentId);
            throw new Exception("Validation error: " + ex.Message, ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error occurred while adding grade for StudentId: {StudentId}", grade.StudentId);
            throw new Exception("Unexpected error occurred while adding the grade.", ex);
        }
    }

    private void ValidateGrade(Grade grade)
    {
        if (string.IsNullOrEmpty(grade.StudentId))
        {
            throw new ArgumentException("StudentId cannot be null or empty.");
        }
        if (string.IsNullOrEmpty(grade.Subject))
        {
            throw new ArgumentException("Subject cannot be null or empty.");
        }
        if (grade.Value < 0 || grade.Value > 100)
        {
            throw new ArgumentException("Value must be between 0 and 100.");
        }
    }
}