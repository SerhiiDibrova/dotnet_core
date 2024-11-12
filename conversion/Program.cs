using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Database connection string is not configured.");
}

builder.Services.AddDbContext<GradeContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddLogging();

var app = builder.Build();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();

public class Grade
{
    public int Id { get; set; }
    public string Subject { get; set; }
    public int Score { get; set; }
}

public class GradeContext : DbContext
{
    public GradeContext(DbContextOptions<GradeContext> options) : base(options) { }

    public DbSet<Grade> Grades { get; set; }
}

[ApiController]
[Route("api/grades")]
public class GradesController : ControllerBase
{
    private readonly GradeContext _context;
    private readonly ILogger<GradesController> _logger;

    public GradesController(GradeContext context, ILogger<GradesController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> AddGrade([FromBody] Grade grade)
    {
        if (grade == null || string.IsNullOrEmpty(grade.Subject) || grade.Score < 0)
        {
            return BadRequest("Invalid grade data.");
        }

        _context.Grades.Add(grade);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "An error occurred while saving the grade.");
            return StatusCode(500, "An error occurred while saving the grade.");
        }

        return CreatedAtAction(nameof(AddGrade), new { id = grade.Id }, grade);
    }
}