package DTOs;

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

public class StudentDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }
}

public class GradeDto
{
    public int Id { get; set; }
    public string Subject { get; set; }
    public decimal Score { get; set; }
    public string Term { get; set; }
}

public class StudentWithGradesDto
{
    public StudentDto Student { get; set; }
    public List<GradeDto> Grades { get; set; }
}

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    [HttpGet("{id}")]
    public ActionResult<StudentWithGradesDto> GetStudentWithGrades(int id)
    {
        if (id <= 0)
        {
            return BadRequest("Invalid student ID.");
        }

        var student = new StudentDto { Id = id, Name = "John Doe", Email = "john.doe@example.com", DateOfBirth = new DateTime(2000, 1, 1), Address = "123 Main St", PhoneNumber = "123-456-7890" };
        if (student == null)
        {
            return NotFound("Student not found.");
        }

        var grades = new List<GradeDto>
        {
            new GradeDto { Id = 1, Subject = "Math", Score = 95, Term = "Fall" },
            new GradeDto { Id = 2, Subject = "Science", Score = 88, Term = "Fall" }
        };

        var response = new StudentWithGradesDto
        {
            Student = student,
            Grades = grades
        };

        return Ok(response);
    }
}