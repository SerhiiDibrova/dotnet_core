namespace YourNamespace.Data
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Infrastructure;
    using Microsoft.EntityFrameworkCore.Storage;
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
        }

        public DbSet<Grade> Grades { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Grade>()
                .HasKey(g => g.Id);

            modelBuilder.Entity<Grade>()
                .Property(g => g.StudentName)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Grade>()
                .Property(g => g.Subject)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Grade>()
                .Property(g => g.Score)
                .IsRequired();
        }

        public async Task<IActionResult> AddGradeAsync(Grade grade)
        {
            if (grade == null)
            {
                return new BadRequestObjectResult("Grade cannot be null.");
            }

            try
            {
                await Grades.AddAsync(grade);
                await SaveChangesAsync();
                return new OkObjectResult("Grade added successfully.");
            }
            catch (DbUpdateException dbEx)
            {
                return new StatusCodeResult(500);
            }
            catch (Exception)
            {
                return new StatusCodeResult(500);
            }
        }
    }

    public class Grade
    {
        public int Id { get; set; }
        public string StudentName { get; set; }
        public string Subject { get; set; }
        public int Score { get; set; }
    }
}