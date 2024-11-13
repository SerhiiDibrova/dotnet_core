package Controllers;

using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using YourNamespace.Services;
using YourNamespace.Dtos;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;
using System.Linq;

[ApiController]
public class StudentsController : ControllerBase
{
    private readonly StudentService _studentService;
    private readonly ILogger<StudentsController> _logger;

    public StudentsController(StudentService studentService, ILogger<StudentsController> logger)
    {
        _studentService = studentService;
        _logger = logger;
    }

    [HttpPost]
    [Route("api/students")]
    public async Task<IActionResult> CreateStudent([FromBody] CreateStudentDto createStudentDto)
    {
        if (createStudentDto == null || !ModelState.IsValid || !IsValidEmail(createStudentDto.Email))
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();
            if (createStudentDto != null && !IsValidEmail(createStudentDto.Email))
            {
                errors.Add("Invalid email format.");
            }
            return BadRequest(new { message = "Validation errors occurred.", errors });
        }

        try
        {
            var student = await _studentService.CreateStudentAsync(createStudentDto);
            return Ok(new 
            { 
                id = student.Id, 
                firstName = student.FirstName, 
                lastName = student.LastName, 
                email = student.Email, 
                dateOfBirth = student.DateOfBirth, 
                enrollmentDate = student.EnrollmentDate 
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while creating the student.");
            return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
        }
    }

    private bool IsValidEmail(string email)
    {
        var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, emailPattern);
    }
}