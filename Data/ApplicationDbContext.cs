namespace YourNamespace.Data
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using System;

    public class ApplicationDbContext : DbContext
    {
        public DbSet<Student> Students { get; set; }

        private readonly ILogger<ApplicationDbContext> _logger;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ILogger<ApplicationDbContext> logger) : base(options)
        {
            _logger = logger;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>()
                .HasKey(s => s.Id);
            modelBuilder.Entity<Student>()
                .Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(100);
            modelBuilder.Entity<Student>()
                .Property(s => s.Age)
                .IsRequired();
        }

        public void AddStudent(Student student)
        {
            if (string.IsNullOrWhiteSpace(student.Name) || student.Age <= 0)
            {
                throw new ArgumentException("Invalid student data.");
            }

            try
            {
                Students.Add(student);
                SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "An error occurred while saving the student.");
                throw new Exception("An error occurred while saving the student.");
            }
        }
    }

    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
    }
}