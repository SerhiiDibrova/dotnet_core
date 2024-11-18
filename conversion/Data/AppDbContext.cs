namespace Conversion.Data
{
    using Microsoft.EntityFrameworkCore;
    using Conversion.Models;

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Student> Students { get; set; } = null!;
        public DbSet<Grade> Grades { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().ToTable("Students");
            modelBuilder.Entity<Grade>().ToTable("Grades");
            modelBuilder.Entity<Student>().HasKey(s => s.Id);
            modelBuilder.Entity<Grade>().HasKey(g => g.Id);
            modelBuilder.Entity<Grade>().HasOne(g => g.Student)
                .WithMany(s => s.Grades)
                .HasForeignKey(g => g.StudentId);
        }
    }
}