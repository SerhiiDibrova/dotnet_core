namespace Conversion.Controllers

using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using YourNamespace.Models;
using YourNamespace.Services;
using System.Text.RegularExpressions;
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

    [HttpPost]
    public async Task<IActionResult> CreateStudent([FromBody] Student student)
    {
        if (student == null)
        {
            return BadRequest("Student object cannot be null.");
        }

        if (string.IsNullOrWhiteSpace(student.Email) || !IsValidEmail(student.Email))
        {
            return BadRequest("Invalid email format.");
        }

        if (student.Age < 18)
        {
            return BadRequest("Student must be at least 18 years old.");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var createdStudent = await _studentService.CreateStudentAsync(student);
            return Ok(createdStudent);
        }
        catch (EmailAlreadyExistsException)
        {
            return Conflict("Email already exists.");
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while creating a student.");
            return StatusCode(500, "Internal server error.");
        }
    }

    private bool IsValidEmail(string email)
    {
        var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, emailPattern);
    }
}