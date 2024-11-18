namespace YourNamespace.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Grade
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Subject { get; set; }

        [Range(0, 100)]
        public int Score { get; set; }

        [Required]
        public int StudentId { get; set; }
    }

    public class StudentWithGradesDto
    {
        [Required]
        public int StudentId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public List<GradeDto> Grades { get; set; } = new List<GradeDto>();
    }

    public class GradeDto
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public int Score { get; set; }
    }

    public class Student
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public List<Grade> Grades { get; set; } = new List<Grade>();
    }
}

namespace YourNamespace.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;
    using System.Threading.Tasks;
    using YourNamespace.Data;

    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public StudentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StudentWithGradesDto>> GetStudentWithGrades(int id)
        {
            var student = await _context.Students
                .Where(s => s.Id == id)
                .Select(s => new StudentWithGradesDto
                {
                    StudentId = s.Id,
                    Name = s.Name,
                    Grades = s.Grades.Select(g => new GradeDto
                    {
                        Id = g.Id,
                        Subject = g.Subject,
                        Score = g.Score
                    }).ToList()
                }).FirstOrDefaultAsync();

            if (student == null)
            {
                return NotFound();
            }

            return student;
        }
    }
}