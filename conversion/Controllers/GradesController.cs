package conversion.Controllers;

using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using conversion.Data;
using conversion.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class GradesController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<GradesController> _logger;

    public GradesController(ApplicationDbContext context, ILogger<GradesController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> AddGrade([FromBody] Grade grade)
    {
        if (grade == null || !ModelState.IsValid)
        {
            return BadRequest("Invalid grade data.");
        }

        if (_context.Grades.Any(g => g.StudentId == grade.StudentId && g.SubjectId == grade.SubjectId))
        {
            return Conflict("Duplicate grade entry for the same student and subject.");
        }

        try
        {
            _context.Grades.Add(grade);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Grade added successfully: {Grade}", grade);
            return Ok(grade);
        }
        catch (DbUpdateException dbEx)
        {
            _logger.LogError(dbEx, "Database update error occurred while adding grade.");
            return StatusCode(500, "Database update error: " + dbEx.InnerException?.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while adding grade.");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}