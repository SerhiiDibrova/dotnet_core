```csharp
namespace YourNamespace.Controllers

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using YourNamespace.Data;
using YourNamespace.Models;

[ApiController]
[Route("api/[controller]")]
public class StudentController : ControllerBase
{
    private readonly AppDbContext _context;

    public StudentController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("grades/{studentId}")]
    public IActionResult GetGradesForStudent(int studentId)
    {
        if (studentId <= 0)
        {
            return BadRequest(new { error = "Invalid studentId." });
        }

        try
        {
            var gradesList = _context.Grades.Where(g => g.StudentId == studentId).ToList();

            if (!gradesList.Any())
            {
                return NotFound(new { error = "No grades found for the specified studentId." });
            }

            return Ok(gradesList.Select(g => new 
            {
                g.GradeId,
                g.StudentId,
                g.Subject,
                g.Score,
                g.Date
            }));
        }
        catch
        {
            return StatusCode(500, new { error = "An error occurred while retrieving grades." });
        }
    }
}
```