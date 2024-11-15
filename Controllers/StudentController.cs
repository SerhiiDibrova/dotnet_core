namespace YourNamespace.Controllers

using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

[ApiController]
[Route("api/[controller]")]
public class StudentController : ControllerBase
{
    private readonly IGradeService _gradeService;
    private readonly ILogger<StudentController> _logger;

    public StudentController(IGradeService gradeService, ILogger<StudentController> logger)
    {
        _gradeService = gradeService;
        _logger = logger;
    }

    [HttpGet("grades/{studentId}")]
    public async Task<IActionResult> GetGradesForStudent(int? studentId)
    {
        if (!studentId.HasValue || studentId <= 0)
        {
            return BadRequest(new { error = "Invalid studentId." });
        }

        try
        {
            var gradesList = await _gradeService.GetGradesForStudentAsync(studentId.Value);
            if (gradesList == null || gradesList.Count == 0)
            {
                return NotFound(new { error = "No grades found for the specified studentId." });
            }

            return Ok(gradesList);
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "An error occurred while processing the request for studentId {StudentId}", studentId);
            return StatusCode(500, new { error = "An error occurred while processing your request." });
        }
    }
}

public interface IGradeService
{
    Task<List<Grade>> GetGradesForStudentAsync(int studentId);
}

public class Grade
{
    public int GradeId { get; set; }
    public int StudentId { get; set; }
    public string Subject { get; set; }
    public decimal Score { get; set; }
    public DateTime Date { get; set; }
}