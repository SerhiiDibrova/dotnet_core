package Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using YourNamespace.Data;
using YourNamespace.Models;

[ApiController]
[Route("api/[controller]")]
public class CreateStudentController : ControllerBase
{
    private readonly YourDbContext _context;
    private readonly ILogger<CreateStudentController> _logger;

    public CreateStudentController(YourDbContext context, ILogger<CreateStudentController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateStudent([FromBody] StudentDto studentDto)
    {
        var validationErrors = new List<string>();

        if (string.IsNullOrWhiteSpace(studentDto.FirstName))
            validationErrors.Add("First name is required");
        if (string.IsNullOrWhiteSpace(studentDto.LastName))
            validationErrors.Add("Last name is required");
        if (string.IsNullOrWhiteSpace(studentDto.Email))
            validationErrors.Add("Email is required");
        if (studentDto.DateOfBirth == default)
            validationErrors.Add("Date of birth is required");
        if (studentDto.EnrollmentDate == default)
            validationErrors.Add("Enrollment date is required");
        if (!new EmailAddressAttribute().IsValid(studentDto.Email))
            validationErrors.Add("Invalid email format");

        if (validationErrors.Any())
        {
            _logger.LogWarning("Validation errors: {Errors}", validationErrors);
            return BadRequest(new { Message = "Validation errors", Errors = validationErrors });
        }

        try
        {
            if (await _context.Students.AnyAsync(s => s.Email == studentDto.Email))
            {
                _logger.LogWarning("Email already exists: {Email}", studentDto.Email);
                return BadRequest(new { Message = "Validation errors", Errors = new List<string> { "Email already exists" } });
            }

            var student = new Student
            {
                FirstName = studentDto.FirstName,
                LastName = studentDto.LastName,
                Email = studentDto.Email,
                DateOfBirth = studentDto.DateOfBirth,
                EnrollmentDate = studentDto.EnrollmentDate
            };

            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Student created successfully: {StudentId}, Data: {@Student}", student.Id, student);
            return Ok(new { student.Id, student.FirstName, student.LastName, student.Email, student.DateOfBirth, student.EnrollmentDate });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating student");
            return StatusCode(500, new { Message = "An error occurred while processing your request." });
        }
    }
}

public class StudentDto
{
    public Guid? Id { get; set; }
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public DateTime DateOfBirth { get; set; }
    [Required]
    public DateTime EnrollmentDate { get; set; }
}