```csharp
namespace YourNamespace.Services

using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YourNamespace.Data;
using YourNamespace.Models;

public class GradeService : IGradeService
{
    private readonly ApplicationDbContext _context;

    public GradeService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> AddGradeAsync(Grade grade)
    {
        if (grade == null)
        {
            return new BadRequestObjectResult("Grade data is required.");
        }

        if (grade.StudentId == null)
        {
            return new BadRequestObjectResult("StudentId is required.");
        }

        if (grade.CourseId == null)
        {
            return new BadRequestObjectResult("CourseId is required.");
        }

        if (grade.Value == null || !(grade.Value is decimal) || (decimal)grade.Value < 0 || (decimal)grade.Value > 100)
        {
            return new BadRequestObjectResult("Value must be a numeric type between 0 and 100.");
        }

        if (string.IsNullOrEmpty(grade.DateAssigned) || !DateTime.TryParse(grade.DateAssigned, out _))
        {
            return new BadRequestObjectResult("DateAssigned must be in a valid date format.");
        }

        var studentExists = await _context.Students.AnyAsync(s => s.Id == grade.StudentId);
        var courseExists = await _context.Courses.AnyAsync(c => c.Id == grade.CourseId);

        if (!studentExists)
        {
            return new BadRequestObjectResult("Student does not exist.");
        }

        if (!courseExists)
        {
            return new BadRequestObjectResult("Course does not exist.");
        }

        try
        {
            _context.Grades.Add(grade);
            await _context.SaveChangesAsync();
            return new OkObjectResult(grade);
        }
        catch (Exception)
        {
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }
}
```