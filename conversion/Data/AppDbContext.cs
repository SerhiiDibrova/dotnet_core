namespace Conversion.Data
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using System.Linq;

    public class AppDbContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Grade> Grades { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>()
                .HasMany(s => s.Grades)
                .WithOne(g => g.Student)
                .HasForeignKey(g => g.StudentId);
        }
    }

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
        public async Task<IActionResult> GetStudentWithGrades(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid student ID.");
            }

            var student = await _context.Students
                .Include(s => s.Grades)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (student == null)
            {
                return NotFound();
            }

            return Ok(student);
        }
    }

    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Grade> Grades { get; set; }
    }

    public class Grade
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string Subject { get; set; }
        public int Score { get; set; }
        public Student Student { get; set; }
    }
}