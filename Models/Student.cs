```csharp
namespace YourNamespace.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
    }

    public class Grade
    {
        public int? Id { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public decimal Value { get; set; }
        public DateTime DateAssigned { get; set; }
    }
}

namespace YourNamespace.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using YourNamespace.Models;
    using YourNamespace.Data;
    using System;
    using System.Globalization;
    using System.Linq;

    [ApiController]
    [Route("api/[controller]")]
    public class GradesController : ControllerBase
    {
        private readonly YourDbContext _context;

        public GradesController(YourDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult AddGrade([FromBody] Grade grade)
        {
            if (grade == null)
            {
                return BadRequest("Grade object is null.");
            }

            if (grade.StudentId <= 0)
            {
                return BadRequest("StudentId must be a positive integer.");
            }

            if (grade.CourseId <= 0)
            {
                return BadRequest("CourseId must be a positive integer.");
            }

            if (grade.Value < 0 || grade.Value > 100)
            {
                return BadRequest("Value must be between 0 and 100.");
            }

            if (!DateTime.TryParse(grade.DateAssigned.ToString(), out _))
            {
                return BadRequest("DateAssigned is required and must be a valid date.");
            }

            if (!_context.Students.Any(s => s.Id == grade.StudentId))
            {
                return BadRequest("Student does not exist.");
            }

            if (!_context.Courses.Any(c => c.Id == grade.CourseId))
            {
                return BadRequest("Course does not exist.");
            }

            try
            {
                _context.Grades.Add(grade);
                _context.SaveChanges();
                return Ok(grade);
            }
            catch
            {
                return StatusCode(500, "An error occurred while saving the grade.");
            }
        }
    }
}
```