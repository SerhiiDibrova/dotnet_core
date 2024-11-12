package Services;

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YourNamespace.Data;
using YourNamespace.Models;
using YourNamespace.DTOs;
using Microsoft.Extensions.Logging;

public class GradeService : IGradeService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<GradeService> _logger;

    public GradeService(ApplicationDbContext context, ILogger<GradeService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Grade> AddGradeAsync(AddGradeDto gradeDto)
    {
        if (gradeDto == null)
        {
            throw new ArgumentNullException(nameof(gradeDto));
        }

        if (string.IsNullOrWhiteSpace(gradeDto.StudentId) || gradeDto.Score < 0 || gradeDto.Score > 100)
        {
            throw new ArgumentException("Invalid grade data. Ensure StudentId is provided and Score is between 0 and 100.");
        }

        if (await _context.Grades.AnyAsync(g => g.StudentId == gradeDto.StudentId && g.Score == gradeDto.Score))
        {
            throw new InvalidOperationException("A grade with the same StudentId and Score already exists.");
        }

        var grade = new Grade
        {
            StudentId = gradeDto.StudentId,
            Score = gradeDto.Score,
            CreatedAt = DateTime.UtcNow
        };

        try
        {
            _context.Grades.Add(grade);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Grade added successfully for StudentId: {StudentId}, Score: {Score}", gradeDto.StudentId, gradeDto.Score);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "An error occurred while adding the grade.");
            throw new Exception("An error occurred while adding the grade.", ex);
        }

        return grade;
    }
}