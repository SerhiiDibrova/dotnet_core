package Data;

using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class AppDbContext : DbContext
{
    public DbSet<Grade> Grades { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public async Task<IEnumerable<Grade>> GetGradesByStudentIdAsync(int studentId)
    {
        if (studentId <= 0)
        {
            throw new ArgumentException("Student ID must be a positive integer.", nameof(studentId));
        }

        var grades = await Grades.Where(g => g.StudentId == studentId).ToListAsync();

        if (grades == null || !grades.Any())
        {
            throw new KeyNotFoundException("No grades found for the specified student ID.");
        }

        return grades;
    }
}

public class Grade
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public string Subject { get; set; }
    public int Score { get; set; }
}

// Startup.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public class Startup
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");
        }

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString));
    }
}