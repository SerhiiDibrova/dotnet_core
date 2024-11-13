package conversion.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using YourNamespace.Services;
using YourNamespace.Models;

[ApiController]
[Route("api/students")]
public class StudentsController : ControllerBase
{
    private readonly IStudentService _studentService;
    private readonly ILogger<StudentsController> _logger;

    public StudentsController(IStudentService studentService, ILogger<StudentsController> logger)
    {
        _studentService = studentService;
        _logger = logger;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateStudent([FromBody] StudentDto studentDto)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            _logger.LogWarning("Validation errors occurred: {Errors}", errors);
            return BadRequest(new { Message = "Validation errors occurred.", Errors = errors });
        }

        try
        {
            var createdStudent = await _studentService.CreateStudentAsync(studentDto);
            _logger.LogInformation("Student created successfully with ID: {Id}, Name: {Name}, Email: {Email}, DateOfBirth: {DateOfBirth}", createdStudent.Id, $"{createdStudent.FirstName} {createdStudent.LastName}", createdStudent.Email, createdStudent.DateOfBirth);
            return Created(string.Empty, new 
            { 
                id = createdStudent.Id, 
                firstName = createdStudent.FirstName, 
                lastName = createdStudent.LastName, 
                email = createdStudent.Email, 
                dateOfBirth = createdStudent.DateOfBirth, 
                enrollmentDate = createdStudent.EnrollmentDate 
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating the student.");
            return StatusCode(500, new { Message = "An unexpected error occurred while processing your request." });
        }
    }
}