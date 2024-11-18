namespace Conversion.Models
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

    public class GradeDto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Subject { get; set; }

        [Range(0, 100)]
        public int Score { get; set; }
    }

    public class StudentWithGradesDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public List<GradeDto> Grades { get; set; }

        [Required]
        public int StudentId { get; set; }
    }
}

namespace Conversion.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Conversion.Models;

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
                    Id = s.Id,
                    Name = s.Name,
                    StudentId = s.Id,
                    Grades = s.Grades.Select(g => new GradeDto
                    {
                        Id = g.Id,
                        Subject = g.Subject,
                        Score = g.Score
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (student == null)
            {
                return NotFound();
            }

            return student;
        }
    }
}