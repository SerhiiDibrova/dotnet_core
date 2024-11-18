namespace YourNamespace.Data
{
    using Microsoft.EntityFrameworkCore;
    using YourNamespace.Models;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().ToTable("Students");
        }

        public async Task<IActionResult> CreateStudentAsync(Student student)
        {
            if (student == null)
            {
                return new BadRequestResult();
            }

            try
            {
                await Students.AddAsync(student);
                await SaveChangesAsync();
                return new OkResult();
            }
            catch
            {
                return new StatusCodeResult(500);
            }
        }
    }
}