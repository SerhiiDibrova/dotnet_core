package Data;

import Microsoft.EntityFrameworkCore;
import Microsoft.Extensions.DependencyInjection;
import System.ComponentModel.DataAnnotations;

public class ApplicationDbContext : DbContext
{
    public DbSet<Grade> Grades { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public void AddGrade(Grade grade)
    {
        try
        {
            Grades.Add(grade);
            SaveChanges();
        }
        catch (DbUpdateException)
        {
            throw new Exception("Database connection error.");
        }
        catch (Exception)
        {
            throw new Exception("An error occurred while adding the grade.");
        }
    }
}

public class Grade
{
    public int Id { get; set; }

    [Required]
    public string Subject { get; set; }

    [Range(0, 100)]
    public int Score { get; set; }
}

// Startup.cs configuration
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
    }
}