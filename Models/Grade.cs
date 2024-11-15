namespace YourNamespace.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Grade
    {
        public int? Id { get; set; }

        [Required]
        public int StudentId { get; set; }

        [Required]
        public int CourseId { get; set; }

        [Required]
        [Range(0, 100)]
        public decimal Value { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateAssigned { get; set; }
    }
}

namespace YourNamespace.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using YourNamespace.Models;
    using System.Threading.Tasks;

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
        public async Task<IActionResult> AddGrade([FromBody] Grade grade)
        {
            if (grade == null)
            {
                return BadRequest("Grade object is null.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var studentExists = await _context.Students.AnyAsync(s => s.Id == grade.StudentId);
            if (!studentExists)
            {
                return BadRequest("Student ID does not exist.");
            }

            var courseExists = await _context.Courses.AnyAsync(c => c.Id == grade.CourseId);
            if (!courseExists)
            {
                return BadRequest("Course ID does not exist.");
            }

            if (grade.Value < 0 || grade.Value > 100)
            {
                return BadRequest("Value must be between 0 and 100.");
            }

            if (grade.DateAssigned > DateTime.Now)
            {
                return BadRequest("DateAssigned cannot be in the future.");
            }

            try
            {
                _context.Grades.Add(grade);
                await _context.SaveChangesAsync();
                return Ok(grade);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Internal server error while saving the grade: {ex.Message}");
            }
        }
    }
}