package conversion.Controllers;

using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

public class StudentController : ControllerBase
{
    private readonly IStudentService _studentService;
    private readonly ILogger<StudentController> _logger;

    public StudentController(IStudentService studentService, ILogger<StudentController> logger)
    {
        _studentService = studentService;
        _logger = logger;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetStudentWithGrades(int id)
    {
        if (id <= 0)
        {
            return BadRequest(new { message = "Invalid student ID." });
        }

        try
        {
            var student = await _studentService.GetStudentWithGradesAsync(id);
            if (student == null)
            {
                return NotFound(new { message = "Student not found." });
            }
            return Ok(new { student });
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving the student.");
            return StatusCode(500, new { message = "Internal Server Error." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred.");
            return StatusCode(500, new { message = "Internal Server Error." });
        }
    }
}