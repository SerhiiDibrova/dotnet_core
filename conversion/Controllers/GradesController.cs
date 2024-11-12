package conversion.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class GradesController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ILogger<GradesController> _logger;

    public GradesController(AppDbContext context, ILogger<GradesController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet("{studentId}")]
    public async Task<IActionResult> GetGradesForStudent(int? studentId)
    {
        if (!studentId.HasValue)
        {
            return BadRequest(new { message = "The studentId is required." });
        }

        if (studentId <= 0)
        {
            return BadRequest(new { message = "The studentId must be a valid positive integer." });
        }

        var grades = await _context.Grades.Where(g => g.StudentId == studentId).ToListAsync();

        if (!grades.Any())
        {
            _logger.LogError("No grades found for studentId: {StudentId}", studentId);
            return NotFound(new { message = "No grades found for the specified student." });
        }

        _logger.LogInformation("Grades retrieved for studentId: {StudentId}, Count: {Count}", studentId, grades.Count);

        return Ok(grades.Select(g => new 
        {
            g.GradeId,
            g.StudentId,
            g.Subject,
            g.Score,
            g.Date
        }));
    }
}