package conversion.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CreateStudentController : ControllerBase
{
    private readonly StudentService _studentService;
    private readonly ILogger<CreateStudentController> _logger;

    public CreateStudentController(StudentService studentService, ILogger<CreateStudentController> logger)
    {
        _studentService = studentService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> CreateStudent([FromBody] CreateStudentDto studentDto)
    {
        if (studentDto == null)
        {
            _logger.LogWarning("Received null studentDto.");
            return BadRequest(new { Errors = new List<string> { "Invalid input." } });
        }

        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            _logger.LogWarning("Validation errors occurred: {Errors}", errors);
            return BadRequest(new { Errors = errors });
        }

        try
        {
            var existingStudent = await _studentService.GetStudentByEmailAsync(studentDto.Email);
            if (existingStudent != null)
            {
                return BadRequest(new { Errors = new List<string> { "Email already exists." } });
            }

            var student = new Student
            {
                FirstName = studentDto.FirstName,
                LastName = studentDto.LastName,
                Email = studentDto.Email,
                DateOfBirth = studentDto.DateOfBirth,
                EnrollmentDate = studentDto.EnrollmentDate
            };

            await _studentService.AddStudentAsync(student);
            _logger.LogInformation("Student created successfully: {Student}", student);
            return CreatedAtAction(nameof(CreateStudent), new { id = student.Id }, new 
            {
                student.Id,
                student.FirstName,
                student.LastName,
                student.Email,
                student.DateOfBirth,
                student.EnrollmentDate
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating the student.");
            return StatusCode(500, new { Message = "An error occurred while processing your request." });
        }
    }
}

public class CreateStudentDto
{
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

public class Student
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public DateTime DateOfBirth { get; set; }
    public DateTime EnrollmentDate { get; set; }
}

public class StudentService
{
    private readonly ApplicationDbContext _context;

    public StudentService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Student> GetStudentByEmailAsync(string email)
    {
        return await _context.Students.AsNoTracking().FirstOrDefaultAsync(s => s.Email == email);
    }

    public async Task AddStudentAsync(Student student)
    {
        await _context.Students.AddAsync(student);
        await _context.SaveChangesAsync();
    }
}

public class ApplicationDbContext : DbContext
{
    public DbSet<Student> Students { get; set; }

    public async Task<int> SaveChangesAsync()
    {
        return await base.SaveChangesAsync();
    }
}