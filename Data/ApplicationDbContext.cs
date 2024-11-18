namespace YourNamespace.Data
{
    using Microsoft.EntityFrameworkCore;
    using YourNamespace.Models;

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>()
                .HasKey(s => s.Id);
            modelBuilder.Entity<Student>()
                .Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(100);
            modelBuilder.Entity<Student>()
                .Property(s => s.Email)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}

namespace YourNamespace.Services
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using YourNamespace.Data;
    using YourNamespace.Models;
    using System.Threading.Tasks;

    public class StudentService
    {
        private readonly ApplicationDbContext _context;

        public StudentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> AddStudentAsync(Student student)
        {
            if (student == null)
            {
                return new BadRequestObjectResult("Student object cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(student.Name) || string.IsNullOrWhiteSpace(student.Email))
            {
                return new BadRequestObjectResult("Student Name and Email are required.");
            }

            try
            {
                await _context.Students.AddAsync(student);
                await _context.SaveChangesAsync();
                return new OkObjectResult(student);
            }
            catch (DbUpdateException ex) when (ex.InnerException != null)
            {
                return new StatusCodeResult(500, $"Database constraint violation: {ex.InnerException.Message}");
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500, $"An unexpected error occurred: {ex.Message}");
            }
        }
    }
}