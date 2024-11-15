namespace YourNamespace.Data
{
    using Microsoft.EntityFrameworkCore;
    using YourNamespace.Models;

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Grade> Grades { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Grade>()
                .HasKey(g => g.Id);

            modelBuilder.Entity<Student>()
                .HasKey(s => s.Id);

            modelBuilder.Entity<Course>()
                .HasKey(c => c.Id);
        }
    }
}

namespace YourNamespace.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using YourNamespace.Data;
    using YourNamespace.Models;
    using System;
    using System.Linq;

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
                return BadRequest("Grade data is required.");
            }

            if (grade.StudentId <= 0)
            {
                return BadRequest("Student ID is required and must be greater than zero.");
            }

            if (grade.CourseId <= 0)
            {
                return BadRequest("Course ID is required and must be greater than zero.");
            }

            if (!decimal.TryParse(grade.Value.ToString(), out var value) || value < 0 || value > 100)
            {
                return BadRequest("Value must be a numeric type between 0 and 100.");
            }

            if (!DateTime.TryParse(grade.DateAssigned, out _))
            {
                return BadRequest("DateAssigned must be in a valid date format (ISO 8601).");
            }

            if (!_context.Students.Any(s => s.Id == grade.StudentId))
            {
                return BadRequest("Student ID does not exist.");
            }

            if (!_context.Courses.Any(c => c.Id == grade.CourseId))
            {
                return BadRequest("Course ID does not exist.");
            }

            try
            {
                _context.Grades.Add(grade);
                _context.SaveChanges();
                return Ok(grade);
            }
            catch (Exception ex)
            {
                // Log the exception (logging mechanism not shown here)
                return StatusCode(500, "An error occurred while saving the grade.");
            }
        }
    }
}