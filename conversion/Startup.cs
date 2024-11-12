package conversion;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
        services.AddControllers();
        services.AddAuthentication();
        services.AddAuthorization(options =>
        {
            options.AddPolicy("StudentPolicy", policy =>
                policy.RequireAuthenticatedUser().RequireClaim("CanViewGrades"));
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
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
            endpoints.MapGet("/grades/{studentId}", async context =>
            {
                var studentId = context.Request.RouteValues["studentId"];
                logger.LogInformation($"Request received for studentId: {studentId}");

                var grades = await GetGradesForStudent(studentId.ToString(), context.Request.Query, context.RequestServices, logger);

                if (grades == null || !grades.Any())
                {
                    logger.LogWarning($"No grades found for studentId: {studentId}");
                    context.Response.StatusCode = 404;
                    await context.Response.WriteAsJsonAsync(new { message = "Grades not found" });
                }
                else
                {
                    logger.LogInformation($"Response for studentId: {studentId} - Status: Success");
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsJsonAsync(grades);
                }
            }).RequireAuthorization("StudentPolicy");
        });
    }

    private async Task<object[]> GetGradesForStudent(string studentId, IQueryCollection query, IServiceProvider services, ILogger logger)
    {
        int pageNumber = int.TryParse(query["pageNumber"], out var page) ? page : 1;
        int pageSize = int.TryParse(query["pageSize"], out var size) ? size : 10;

        using (var scope = services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            try
            {
                var grades = await context.Grades
                    .Where(g => g.StudentId == studentId)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToArrayAsync();
                logger.LogInformation($"Retrieved {grades.Length} grades for studentId: {studentId}");
                return grades;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error retrieving grades for studentId: {studentId}");
                throw;
            }
        }
    }
}