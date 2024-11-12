package conversion;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Linq;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        var connectionString = Configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("Database connection string is not configured.");
        }

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString));
        services.AddControllers();
        services.AddLogging();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ILogger<StudentsController> _logger;

    public StudentsController(AppDbContext context, ILogger<StudentsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<StudentWithGradesDto>> GetStudentWithGrades(int id)
    {
        try
        {
            var student = await _context.Students
                .Include(s => s.Grades)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (student == null)
            {
                return NotFound($"Student with ID {id} not found.");
            }

            var studentWithGrades = new StudentWithGradesDto
            {
                Id = student.Id,
                Name = student.Name,
                Grades = student.Grades.Select(g => new GradeDto
                {
                    Subject = g.Subject,
                    Score = g.Score
                }).ToList()
            };

            return Ok(studentWithGrades);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving student with grades for student ID {Id}.", id);
            return StatusCode(500, "Internal server error");
        }
    }
}

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Student> Students { get; set; }
    public DbSet<Grade> Grades { get; set; }
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
    public string Subject { get; set; }
    public int Score { get; set; }
}

public class StudentWithGradesDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<GradeDto> Grades { get; set; }
}

public class GradeDto
{
    public string Subject { get; set; }
    public int Score { get; set; }
}