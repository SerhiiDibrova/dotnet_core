package conversion;

import Microsoft.EntityFrameworkCore;
import Microsoft.Extensions.DependencyInjection;
import Microsoft.AspNetCore.Builder;
import Microsoft.AspNetCore.Hosting;
import Microsoft.Extensions.Configuration;
import Microsoft.Extensions.Logging;

public class Startup {
    public Startup(IConfiguration configuration) {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services) {
        services.AddDbContext<MyDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
        services.AddScoped<IStudentService, StudentService>();
        services.AddLogging();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger) {
        if (env.IsDevelopment()) {
            app.UseDeveloperExceptionPage();
        } else {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(endpoints -> {
            endpoints.MapControllers();
        });
    }
}