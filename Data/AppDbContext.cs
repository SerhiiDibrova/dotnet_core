namespace YourNamespace.Data
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using YourNamespace.Models;
    using System.Linq;

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Grade> Grades { get; set; }
        public DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class GradesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public GradesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult AddGrade([FromBody] Grade grade)
        {
            if (grade == null)
            {
                return BadRequest("Grade cannot be null.");
            }

            if (string.IsNullOrEmpty(grade.Subject) || grade.Score < 0 || grade.Score > 100)
            {
                return BadRequest("Invalid grade properties.");
            }

            try
            {
                if (_context.Grades.Any(g => g.Subject == grade.Subject && g.StudentId == grade.StudentId))
                {
                    return Conflict("Duplicate grade entry.");
                }

                _context.Grades.Add(grade);
                _context.SaveChanges();
                return CreatedAtAction(nameof(AddGrade), new { id = grade.Id }, grade);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Database update error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}