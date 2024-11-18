```csharp
namespace YourNamespace.Services

using System;
using System.Threading.Tasks;
using YourNamespace.Data;
using YourNamespace.Models;
using YourNamespace.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public class GradeService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<GradeService> _logger;

    public GradeService(ApplicationDbContext context, ILogger<GradeService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Grade> AddGradeAsync(GradeDto gradeDto)
    {
        ValidateGradeDto(gradeDto);

        var grade = MapGradeDtoToGrade(gradeDto);

        try
        {
            _context.Grades.Add(grade);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "An error occurred while adding the grade for StudentId: {StudentId}, SubjectId: {SubjectId}", gradeDto.StudentId, gradeDto.SubjectId);
            throw new CustomException("An error occurred while adding the grade.", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while adding the grade for StudentId: {StudentId}, SubjectId: {SubjectId}", gradeDto.StudentId, gradeDto.SubjectId);
            throw new CustomException("An unexpected error occurred while adding the grade.", ex);
        }

        return grade;
    }

    private void ValidateGradeDto(GradeDto gradeDto)
    {
        if (gradeDto == null)
        {
            throw new ArgumentNullException(nameof(gradeDto), "GradeDto cannot be null.");
        }

        if (gradeDto.Score < 0 || gradeDto.Score > 100)
        {
            throw new ArgumentOutOfRangeException(nameof(gradeDto.Score), "Score must be between 0 and 100.");
        }

        if (string.IsNullOrEmpty(gradeDto.StudentId))
        {
            throw new ArgumentException("StudentId is required.", nameof(gradeDto.StudentId));
        }

        if (string.IsNullOrEmpty(gradeDto.SubjectId))
        {
            throw new ArgumentException("SubjectId is required.", nameof(gradeDto.SubjectId));
        }
    }

    private Grade MapGradeDtoToGrade(GradeDto gradeDto)
    {
        return new Grade
        {
            StudentId = gradeDto.StudentId,
            SubjectId = gradeDto.SubjectId,
            Score = gradeDto.Score,
            CreatedAt = DateTime.UtcNow
        };
    }
}

public class CustomException : Exception
{
    public CustomException(string message, Exception innerException) : base(message, innerException) { }
}
```