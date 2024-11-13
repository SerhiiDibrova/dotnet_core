package DTOs;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

public class StudentWithGradesDto
{
    [Required]
    [JsonPropertyName("Student")]
    public StudentDto Student { get; set; }

    [Required]
    [JsonPropertyName("Grades")]
    public List<GradeDto> Grades { get; set; } = new List<GradeDto>();
}

public class StudentDto
{
    [Required]
    [JsonPropertyName("Id")]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    [JsonPropertyName("Name")]
    public string Name { get; set; }

    [Required]
    [EmailAddress]
    [JsonPropertyName("Email")]
    public string Email { get; set; }

    [Required]
    [JsonPropertyName("DateOfBirth")]
    public DateTime DateOfBirth { get; set; }

    [JsonPropertyName("Address")]
    public string Address { get; set; }

    [JsonPropertyName("PhoneNumber")]
    public string PhoneNumber { get; set; }
}

public class GradeDto
{
    [Required]
    [JsonPropertyName("Id")]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    [JsonPropertyName("Subject")]
    public string Subject { get; set; }

    [Required]
    [Range(0, 100)]
    [JsonPropertyName("Score")]
    public decimal Score { get; set; }

    [Required]
    [JsonPropertyName("Term")]
    public string Term { get; set; }
}

public class StudentController : ControllerBase
{
    private readonly IStudentService _studentService;
    private readonly ILogger<StudentController> _logger;

    public StudentController(IStudentService studentService, ILogger<StudentController> logger)
    {
        _studentService = studentService;
        _logger = logger;
    }

    [HttpGet("GetStudentWithGrades/{studentId}")]
    public ActionResult<StudentWithGradesDto> GetStudentWithGrades(int studentId)
    {
        if (studentId <= 0)
        {
            return BadRequest("Invalid student ID.");
        }

        try
        {
            var studentWithGrades = _studentService.GetStudentWithGrades(studentId);
            if (studentWithGrades == null)
            {
                return NotFound();
            }
            return Ok(studentWithGrades);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving student with grades.");
            return StatusCode(500, "Internal server error.");
        }
    }
}

public interface IStudentService
{
    StudentWithGradesDto GetStudentWithGrades(int studentId);
}