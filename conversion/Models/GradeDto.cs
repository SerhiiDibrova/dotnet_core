namespace Conversion.Models
{
    public class GradeDto
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public decimal Score { get; set; }
    }
}

namespace Conversion.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using System.Threading.Tasks;
    using Conversion.Models;
    using Conversion.Data;

    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StudentsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudentWithGrades(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid student ID.");
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound(new { Message = "Student not found." });
            }

            var grades = await _context.Grades
                .Where(g => g.StudentId == id)
                .Select(g => new GradeDto
                {
                    Id = g.Id,
                    Subject = g.Subject,
                    Score = g.Score
                })
                .ToListAsync();

            var response = new
            {
                StudentId = student.Id,
                StudentName = student.Name,
                Grades = grades
            };

            return Ok(response);
        }
    }
}