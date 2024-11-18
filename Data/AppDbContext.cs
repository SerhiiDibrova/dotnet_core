namespace YourNamespace.Data
{
    using Microsoft.EntityFrameworkCore;
    using YourNamespace.Models;
    using System.Threading.Tasks;

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Student> Students { get; set; }
        public DbSet<Grade> Grades { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>()
                .HasMany(s => s.Grades)
                .WithOne(g => g.Student)
                .HasForeignKey(g => g.StudentId);
        }

        public async Task<Student> GetStudentWithGradesAsync(int studentId)
        {
            return await Students.Include(s => s.Grades)
                                 .FirstOrDefaultAsync(s => s.Id == studentId);
        }
    }
}