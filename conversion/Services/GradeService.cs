package conversion.Services;

using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using conversion.Data;
using conversion.Models;
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
        if (gradeDto.StudentId == null || gradeDto.CourseId == null || gradeDto.Value < 0 || gradeDto.Value > 100 || gradeDto.DateAssigned == null)
        {
            throw new ArgumentException("Invalid input data.");
        }

        var studentExists = await _context.Students.AnyAsync(s => s.Id == gradeDto.StudentId);
        var courseExists = await _context.Courses.AnyAsync(c => c.Id == gradeDto.CourseId);

        if (!studentExists || !courseExists)
        {
            throw new ArgumentException("Student or Course does not exist.");
        }

        if (!DateTime.TryParse(gradeDto.DateAssigned.ToString(), out var dateAssigned) || dateAssigned > DateTime.Now)
        {
            throw new ArgumentException("Invalid date format or date is in the future.");
        }

        var grade = new Grade
        {
            StudentId = gradeDto.StudentId,
            CourseId = gradeDto.CourseId,
            Value = gradeDto.Value,
            DateAssigned = dateAssigned
        };

        try
        {
            _context.Grades.Add(grade);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "An error occurred while saving the grade.");
            throw new Exception("An error occurred while saving the grade.");
        }

        return grade;
    }
}