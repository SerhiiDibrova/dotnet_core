package conversion.Services;

using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using conversion.Data;
using conversion.Models;
using Microsoft.Extensions.Logging;

public class GradeService
{
    private readonly AppDbContext _context;
    private readonly ILogger<GradeService> _logger;

    public GradeService(AppDbContext context, ILogger<GradeService> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IActionResult> AddGradeAsync(GradeDto gradeDto)
    {
        if (gradeDto == null || 
            string.IsNullOrEmpty(gradeDto.StudentId) || 
            string.IsNullOrEmpty(gradeDto.CourseId) || 
            gradeDto.Value == null || 
            string.IsNullOrEmpty(gradeDto.DateAssigned))
        {
            _logger.LogWarning("Missing required fields.");
            return new BadRequestObjectResult("Missing required fields.");
        }

        var studentExists = await _context.Students.AnyAsync(s => s.Id == gradeDto.StudentId);
        var courseExists = await _context.Courses.AnyAsync(c => c.Id == gradeDto.CourseId);

        if (!studentExists || !courseExists)
        {
            _logger.LogWarning("Invalid StudentId or CourseId.");
            return new BadRequestObjectResult("Invalid StudentId or CourseId.");
        }

        if (!double.TryParse(gradeDto.Value.ToString(), out double value) || value < 0 || value > 100)
        {
            _logger.LogWarning("Value must be a number between 0 and 100.");
            return new BadRequestObjectResult("Value must be a number between 0 and 100.");
        }

        if (!DateTime.TryParseExact(gradeDto.DateAssigned, new[] { "yyyy-MM-dd", "MM/dd/yyyy", "dd-MM-yyyy" }, 
            CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateAssigned))
        {
            _logger.LogWarning("DateAssigned is not in a valid date format.");
            return new BadRequestObjectResult("DateAssigned is not in a valid date format. Use 'yyyy-MM-dd', 'MM/dd/yyyy', or 'dd-MM-yyyy'.");
        }

        var grade = new Grade
        {
            StudentId = gradeDto.StudentId,
            CourseId = gradeDto.CourseId,
            Value = value,
            DateAssigned = dateAssigned
        };

        try
        {
            await _context.Grades.AddAsync(grade);
            await _context.SaveChangesAsync();
            return new OkObjectResult(grade);
        }
        catch
        {
            _logger.LogError("An error occurred while saving the grade.");
            return new StatusCodeResult(500, "Internal server error.");
        }
    }
}