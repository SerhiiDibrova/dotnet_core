namespace Conversion.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;
    using Conversion.Services;
    using Conversion.DTOs;

    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpPost("add-grade")]
        public async Task<IActionResult> AddGrade([FromBody] GradeDto grade)
        {
            if (grade == null)
            {
                return BadRequest("Grade data is required.");
            }

            if (string.IsNullOrEmpty(grade.StudentId) || string.IsNullOrEmpty(grade.CourseId) || 
                grade.Value == null || grade.DateAssigned == default)
            {
                return BadRequest("StudentId, CourseId, Value, and DateAssigned are required.");
            }

            if (!await _studentService.StudentExists(grade.StudentId))
            {
                return BadRequest("Student does not exist.");
            }

            if (!await _studentService.CourseExists(grade.CourseId))
            {
                return BadRequest("Course does not exist.");
            }

            if (!decimal.TryParse(grade.Value.ToString(), out var numericValue) || numericValue < 0 || numericValue > 100)
            {
                return BadRequest("Value must be a numeric type between 0 and 100.");
            }

            if (grade.DateAssigned > DateTime.Now || grade.DateAssigned < new DateTime(1900, 1, 1))
            {
                return BadRequest("DateAssigned must be a valid date and cannot be in the future.");
            }

            try
            {
                await _studentService.AddGradeAsync(grade);
                return Ok("Grade added successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while adding the grade: {ex.Message}");
            }
        }
    }
}