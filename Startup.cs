```csharp
namespace YourNamespace
{
    using FluentValidation;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using YourNamespace.Data;
    using YourNamespace.Services;

    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>();
            services.AddScoped<StudentService>();
            services.AddControllers();
            services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<StudentValidator>());
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
}

namespace YourNamespace.Services
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using YourNamespace.Data;
    using YourNamespace.Models;
    using FluentValidation;

    public class StudentService
    {
        private readonly ApplicationDbContext _context;
        private readonly IValidator<Student> _validator;

        public StudentService(ApplicationDbContext context, IValidator<Student> validator)
        {
            _context = context;
            _validator = validator;
        }

        public async Task<IActionResult> CreateStudentAsync(Student student)
        {
            if (student == null)
            {
                return new BadRequestObjectResult("Student object cannot be null.");
            }

            var validationResult = await _validator.ValidateAsync(student);
            if (!validationResult.IsValid)
            {
                return new BadRequestObjectResult(validationResult.Errors);
            }

            try
            {
                _context.Students.Add(student);
                await _context.SaveChangesAsync();
                return new OkObjectResult(student);
            }
            catch (Exception ex)
            {
                // Log the exception (logging mechanism not shown here)
                return new StatusCodeResult(500);
            }
        }
    }
}

namespace YourNamespace.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
    }
}

namespace YourNamespace.Validators
{
    using FluentValidation;
    using YourNamespace.Models;

    public class StudentValidator : AbstractValidator<Student>
    {
        public StudentValidator()
        {
            RuleFor(student => student.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(student => student.Age).GreaterThan(0).WithMessage("Age must be greater than 0.");
        }
    }
}
```