namespace YourNamespace.Data
{
    using Microsoft.EntityFrameworkCore;
    using YourNamespace.Models;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Student> Students { get; set; }
        public DbSet<Grade> Grades { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().ToTable("Students");
            modelBuilder.Entity<Grade>().ToTable("Grades");
        }

        public async Task<Student> GetStudentWithGradesAsync(int studentId)
        {
            if (studentId <= 0)
            {
                throw new ArgumentException("Invalid student ID");
            }

            var student = await Students
                .Include(s => s.Grades)
                .FirstOrDefaultAsync(s => s.Id == studentId);

            if (student == null)
            {
                throw new KeyNotFoundException("Student not found");
            }

            return student;
        }
    }
}