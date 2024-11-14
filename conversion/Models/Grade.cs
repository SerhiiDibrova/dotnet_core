namespace Conversion.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Grade
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "StudentId is required.")]
        public int StudentId { get; set; }

        [Required(ErrorMessage = "CourseId is required.")]
        public int CourseId { get; set; }

        [Required(ErrorMessage = "Value is required.")]
        [Range(0, 100, ErrorMessage = "Value must be between 0 and 100.")]
        public decimal Value { get; set; }

        [Required(ErrorMessage = "DateAssigned is required.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateAssigned { get; set; }
    }
}

namespace Conversion.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using Conversion.Models;
    using Microsoft.EntityFrameworkCore;

    [ApiController]
    [Route("api/[controller]")]
    public class GradesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public GradesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> AddGrade([FromBody] Grade grade)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await StudentExists(grade.StudentId))
            {
                return BadRequest("Invalid StudentId. The specified student does not exist.");
            }

            if (!await CourseExists(grade.CourseId))
            {
                return BadRequest("Invalid CourseId. The specified course does not exist.");
            }

            if (grade.DateAssigned == default)
            {
                return BadRequest("DateAssigned must be a valid date.");
            }

            try
            {
                _context.Grades.Add(grade);
                await _context.SaveChangesAsync();
                return Ok(new { Id = grade.Id ?? 0, grade.StudentId, grade.CourseId, grade.Value, grade.DateAssigned });
            }
            catch
            {
                return StatusCode(500, "Internal server error.");
            }
        }

        private async Task<bool> StudentExists(int studentId)
        {
            return await _context.Students.AnyAsync(s => s.Id == studentId);
        }

        private async Task<bool> CourseExists(int courseId)
        {
            return await _context.Courses.AnyAsync(c => c.Id == courseId);
        }
    }
}