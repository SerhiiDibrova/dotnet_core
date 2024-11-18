namespace YourNamespace.Controllers

using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using YourNamespace.Services;
using YourNamespace.Models;
using Microsoft.Extensions.Logging;

[ApiController]
[Route("api/[controller]")]
public class StudentController : ControllerBase
{
    private readonly IStudentService _studentService;
    private readonly ILogger<StudentController> _logger;

    public StudentController(IStudentService studentService, ILogger<StudentController> logger)
    {
        _studentService = studentService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllStudents()
    {
        try
        {
            var students = await _studentService.GetAllStudentsAsync();
            if (students == null || students.Count == 0)
            {
                return Ok(new List<Student>());
            }
            return Ok(students);
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving students.");
            return StatusCode(500, new { message = "An error occurred while retrieving student data." });
        }
    }
}