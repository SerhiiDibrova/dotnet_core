namespace YourNamespace.Controllers

using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using YourNamespace.Services;
using YourNamespace.Models;

[ApiController]
[Route("api/[controller]")]
public class StudentController : ControllerBase
{
    private readonly IStudentService _studentService;

    public StudentController(IStudentService studentService)
    {
        _studentService = studentService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateStudent([FromBody] CreateStudentDto studentDto)
    {
        if (studentDto == null)
        {
            return BadRequest("Student data is required.");
        }

        if (string.IsNullOrWhiteSpace(studentDto.FirstName))
        {
            return BadRequest("First name is required.");
        }

        if (string.IsNullOrWhiteSpace(studentDto.LastName))
        {
            return BadRequest("Last name is required.");
        }

        if (string.IsNullOrWhiteSpace(studentDto.Email))
        {
            return BadRequest("Email is required.");
        }

        if (studentDto.DateOfBirth == default)
        {
            return BadRequest("Date of birth is required.");
        }

        if (studentDto.EnrollmentDate == default)
        {
            return BadRequest("Enrollment date is required.");
        }

        if (!IsValidEmail(studentDto.Email))
        {
            return BadRequest("Invalid email format.");
        }

        if (await _studentService.EmailExistsAsync(studentDto.Email))
        {
            return Conflict("Email already exists.");
        }

        if (!IsAdult(studentDto.DateOfBirth))
        {
            return BadRequest("Student must be at least 18 years old.");
        }

        var student = new Student
        {
            FirstName = studentDto.FirstName,
            LastName = studentDto.LastName,
            Email = studentDto.Email,
            DateOfBirth = studentDto.DateOfBirth,
            EnrollmentDate = studentDto.EnrollmentDate
        };

        try
        {
            var createdStudent = await _studentService.CreateStudentAsync(student);
            return Ok(createdStudent);
        }
        catch
        {
            return StatusCode(500, "An unexpected error occurred.");
        }
    }

    private bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    private bool IsAdult(DateTime dateOfBirth)
    {
        var age = DateTime.Now.Year - dateOfBirth.Year;
        if (dateOfBirth > DateTime.Now.AddYears(-age)) age--;
        return age >= 18;
    }
}