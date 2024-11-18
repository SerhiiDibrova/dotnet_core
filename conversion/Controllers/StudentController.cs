namespace Conversion.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using System.Threading.Tasks;

    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StudentController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudentWithGrades(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new { Message = "Invalid student ID." });
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound(new { Message = "No data found." });
            }

            var grades = await _context.Grades
                .Where(g => g.StudentId == id)
                .ToListAsync();

            return Ok(new { Student = student, Grades = grades });
        }
    }
}