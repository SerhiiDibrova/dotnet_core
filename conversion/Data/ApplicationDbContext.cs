namespace Conversion.Data
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ApplicationDbContext : DbContext
    {
        public DbSet<Grade> Grades { get; set; } = null!;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Grade>()
                .HasKey(g => g.Id);
            modelBuilder.Entity<Grade>()
                .Property(g => g.Subject)
                .IsRequired();
        }

        public IEnumerable<Grade> GetGradesByStudentId(int studentId)
        {
            if (studentId <= 0)
            {
                throw new ArgumentException("Invalid student ID", nameof(studentId));
            }
            return Grades.Where(g => g.StudentId == studentId).ToList();
        }
    }

    public class Grade
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string Subject { get; set; }
        public int Score { get; set; }
    }
}