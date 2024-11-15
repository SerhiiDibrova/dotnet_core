```csharp
namespace YourNamespace.Controllers

using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using System.Threading.Tasks;
using YourNamespace.Data;
using YourNamespace.Models;

[Route("api/[controller]")]
[ApiController]
public class GradeController : ControllerBase
{
    private readonly YourDbContext _context;

    public GradeController(YourDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> AddGrade([FromBody] Grade grade)
    {
        if (grade == null)
        {
            return BadRequest("Grade object is null.");
        }

        if (grade.StudentId <= 0)
        {
            return BadRequest("StudentId is required and must be greater than zero.");
        }

        if (grade.CourseId <= 0)
        {
            return BadRequest("CourseId is required and must be greater than zero.");
        }

        if (grade.Value < 0 || grade.Value > 100)
        {
            return BadRequest("Value must be between 0 and 100.");
        }

        if (string.IsNullOrWhiteSpace(grade.DateAssigned) || !DateTime.TryParseExact(grade.DateAssigned, "o", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateAssigned))
        {
            return BadRequest("DateAssigned must be in ISO 8601 format.");
        }

        var studentExists = await _context.Students.FindAsync(grade.StudentId) != null;
        var courseExists = await _context.Courses.FindAsync(grade.CourseId) != null;

        if (!studentExists || !courseExists)
        {
            return BadRequest("Student or Course does not exist.");
        }

        grade.DateAssigned = dateAssigned;

        try
        {
            _context.Grades.Add(grade);
            await _context.SaveChangesAsync();
            return Ok(grade);
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal server error.");
        }
    }
}
```