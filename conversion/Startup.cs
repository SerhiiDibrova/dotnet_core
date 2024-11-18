namespace Conversion
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<IStudentService, StudentService>();
            services.AddControllers();
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

    public class StudentService : IStudentService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<StudentService> _logger;

        public StudentService(ApplicationDbContext context, ILogger<StudentService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> CreateStudentAsync(Student student)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(student);

            if (!Validator.TryValidateObject(student, validationContext, validationResults, true))
            {
                return new BadRequestObjectResult(validationResults);
            }

            try
            {
                _context.Students.Add(student);
                await _context.SaveChangesAsync();
                return new OkResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while saving the student.");
                return new StatusCodeResult(500);
            }
        }
    }

    public class Student
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Age { get; set; }
        [Required]
        public string Email { get; set; }
    }

    public interface IStudentService
    {
        Task<IActionResult> CreateStudentAsync(Student student);
    }

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Student> Students { get; set; }
    }
}