package conversion.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using YourNamespace.Services;
using Microsoft.Extensions.Logging;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class StudentsController : ControllerBase
{
    private readonly IStudentService _studentService;
    private readonly ILogger<StudentsController> _logger;

    public StudentsController(IStudentService studentService, ILogger<StudentsController> logger)
    {
        _studentService = studentService;
        _logger = logger;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetStudentWithGrades(string id)
    {
        if (string.IsNullOrWhiteSpace(id) || !int.TryParse(id, out int studentId) || studentId <= 0)
        {
            return BadRequest("The 'id' parameter is required and must be a valid integer.");
        }

        try
        {
            var student = await _studentService.GetStudentWithGradesAsync(studentId);
            if (student == null)
            {
                return NotFound($"No student exists with the specified id: {studentId}.");
            }

            return Ok(student);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Argument null exception occurred while retrieving student with id: {Id}", studentId);
            return StatusCode(500, "An unexpected error occurred while processing your request.");
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Invalid operation exception occurred while retrieving student with id: {Id}", studentId);
            return StatusCode(500, "An unexpected error occurred while processing your request.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while retrieving student with id: {Id}", studentId);
            return StatusCode(500, "An unexpected error occurred while processing your request.");
        }
    }
}