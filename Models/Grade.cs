```csharp
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YourNamespace.Data;
using YourNamespace.Models;

namespace YourNamespace.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StudentsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public ActionResult<StudentWithGradesDto> GetStudentWithGrades(int id)
        {
            var student = _context.Students
                .Include(s => s.Grades)
                .FirstOrDefault(s => s.Id == id);

            if (student == null)
            {
                return NotFound();
            }

            var studentWithGrades = new StudentWithGradesDto
            {
                Id = student.Id,
                Name = student.Name,
                Grades = student.Grades.Select(g => new GradeDto
                {
                    Id = g.Id,
                    Subject = g.Subject,
                    Score = g.Score
                }).ToList()
            };

            return Ok(studentWithGrades);
        }
    }

    public class StudentWithGradesDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<GradeDto> Grades { get; set; }
    }

    public class GradeDto
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public int Score { get; set; }
    }
}
```