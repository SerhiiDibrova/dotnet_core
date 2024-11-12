using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YourNamespace
{
    public class AppDbContext : DbContext
    {
        public DbSet<Grade> Grades { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }

    public class Grade
    {
        public int GradeId { get; set; }
        public int StudentId { get; set; }
        public string Subject { get; set; }
        public decimal Score { get; set; }
        public DateTime Date { get; set; }
    }

    public static class Startup
    {
        public static void ConfigureServices(IServiceCollection services, string connectionString)
        {
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
            services.AddControllers();
            services.AddAuthorization();
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

        [HttpGet("{studentId}")]
        [Authorize]
        public async Task<IActionResult> GetGradesForStudent(int studentId)
        {
            if (studentId <= 0)
            {
                return BadRequest(new { error = "Invalid student ID." });
            }

            var grades = await _context.Grades.Where(g => g.StudentId == studentId).ToListAsync();

            if (grades == null || !grades.Any())
            {
                return NotFound(new { error = "No grades found for the specified student." });
            }

            return Ok(grades.Select(g => new 
            {
                g.GradeId,
                g.StudentId,
                g.Subject,
                g.Score,
                g.Date
            }));
        }
    }
}