namespace Conversion.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;
    using System.Threading.Tasks;
    using Conversion.Data;
    using Conversion.Models;
    using Conversion.Models.Responses;
    using Microsoft.EntityFrameworkCore;

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
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound(new ErrorResponse { Message = "Student not found." });
            }

            var grades = await _context.Grades.Where(g => g.StudentId == id).ToListAsync();
            var response = new
            {
                Student = student,
                Grades = grades
            };

            return Ok(response);
        }
    }
}