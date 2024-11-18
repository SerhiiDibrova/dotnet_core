```csharp
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using YourNamespace.Models;
using YourNamespace.Data;

namespace YourNamespace.DTOs
{
    public class GradeDto
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public decimal Score { get; set; }
    }

    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public StudentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudentWithGrades(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            var grades = await _context.Grades
                .Where(g => g.StudentId == id)
                .Select(g => new GradeDto
                {
                    Id = g.Id,
                    Subject = g.Subject,
                    Score = g.Score
                }).ToListAsync();

            return Ok(new
            {
                Student = new
                {
                    Id = student.Id,
                    Name = student.Name,
                    Email = student.Email
                },
                Grades = grades
            });
        }
    }
}
```