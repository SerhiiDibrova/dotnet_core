package Data;

import Microsoft.EntityFrameworkCore;
import System.ComponentModel.DataAnnotations;

public class ApplicationDbContext : DbContext
{
    public virtual DbSet<Grade> Grades { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
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

// Startup.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
    }
}