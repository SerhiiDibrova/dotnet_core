namespace Conversion.Controllers

using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Conversion.Services;
using Conversion.Models;

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
    public async Task<IActionResult> CreateStudent([FromBody] CreateStudentRequest request)
    {
        if (request.Id.HasValue && request.Id.Value <= 0)
        {
            return BadRequest(new { message = "Id must be a positive integer." });
        }
        if (string.IsNullOrWhiteSpace(request.FirstName))
        {
            return BadRequest(new { message = "FirstName is required." });
        }
        if (string.IsNullOrWhiteSpace(request.LastName))
        {
            return BadRequest(new { message = "LastName is required." });
        }
        if (string.IsNullOrWhiteSpace(request.Email) || !Regex.IsMatch(request.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        {
            return BadRequest(new { message = "A valid Email is required." });
        }
        if (request.DateOfBirth == default)
        {
            return BadRequest(new { message = "DateOfBirth is required." });
        }
        if (request.EnrollmentDate == default || request.EnrollmentDate > DateTime.UtcNow)
        {
            return BadRequest(new { message = "EnrollmentDate is required and cannot be in the future." });
        }

        var existingStudent = await _studentService.GetStudentByEmailAsync(request.Email);
        if (existingStudent != null)
        {
            return Conflict(new { message = "Email already exists." });
        }

        try
        {
            var student = new Student
            {
                Id = request.Id,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                DateOfBirth = request.DateOfBirth,
                EnrollmentDate = request.EnrollmentDate
            };

            var createdStudent = await _studentService.CreateStudentAsync(student);
            return Ok(createdStudent);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Internal server error.", details = ex.Message });
        }
    }
}