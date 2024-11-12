package conversion;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(Environment.GetEnvironmentVariable("ConnectionString")));

        services.AddScoped<IGradeService, GradeService>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Environment.GetEnvironmentVariable("Issuer"),
                    ValidAudience = Environment.GetEnvironmentVariable("Audience"),
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SecretKey")))
                };
            });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("GradeAccess", policy =>
                policy.RequireClaim("Permission", "ViewGrades"));
        });

        services.AddControllers();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}

using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("grades")]
public class GradesController : ControllerBase
{
    private readonly IGradeService _gradeService;

    public GradesController(IGradeService gradeService)
    {
        _gradeService = gradeService;
    }

    [HttpGet("{studentId}")]
    [Authorize(Policy = "GradeAccess")]
    public async Task<IActionResult> GetGradesForStudent(string studentId)
    {
        if (string.IsNullOrWhiteSpace(studentId))
        {
            return BadRequest("Invalid student ID.");
        }

        var grades = await _gradeService.GetGradesForStudentAsync(studentId);
        return Ok(grades);
    }
}