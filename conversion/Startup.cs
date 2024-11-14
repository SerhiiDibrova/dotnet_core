```csharp
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RazorPages;

namespace conversion
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddControllersWithViews(options =>
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });
            
            // Add Razor Pages support.
            services.AddRazorPages();

            // Configure routing options.
            services.AddRouting(options => options.LowercaseUrls = true);

            // Configure CORS policy to allow specific origins and validate requirements.
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigins",
                    builder =>
                    {
                        builder.WithOrigins("https://localhost:5001")
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials();
                    });
            });

            // Add validation for CORS policy requirements.
            services.AddSingleton<ICorsPolicyProvider, CorsPolicyProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                // Enable Developer Exception Page for development environment.
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // Enable HSTS for production environment.
                app.UseHsts();

                // Add error handling middleware.
                app.UseExceptionHandler(appError =>
                {
                    appError.Run(async context =>
                    {
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("Internal Server Error.");
                    });
                });
            }

            // Redirect HTTP requests to HTTPS.
            app.UseHttpsRedirection();

            // Use routing middleware.
            app.UseRouting();

            // Use CORS policy with validation.
            app.UseCors("AllowSpecificOrigins");

            // Map endpoints for controllers and Razor Pages.
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });
        }
    }

    public class CorsPolicyProvider : ICorsPolicyProvider
    {
        private readonly CorsOptions _options;

        public CorsPolicyProvider(IOptions<CorsOptions> options)
        {
            _options = options.Value;
        }

        public Task<CorsPolicy> GetPolicyAsync(HttpContext context, string policyName)
        {
            if (_options.GetPolicies().TryGetValue(policyName, out var corsPolicy))
            {
                return Task.FromResult(corsPolicy);
            }
            else
            {
                throw new ArgumentException("Invalid CORS policy name.");
            }
        }
    }
}
```